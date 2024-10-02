using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.UnitOfWork;


namespace MyStore.Server.Models.Service.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStripeService _stripeService;

        public OrderService(IUnitOfWork unitOfWork, IStripeService stripeService)
        {
            _unitOfWork = unitOfWork;
            _stripeService = stripeService;
        }

        public async Task<IEnumerable<OrderResultModel>> GetOrdersAsync(int memberId)
        {
            var orderEnum = await _unitOfWork.OrderRepository.GetEnumAsync(memberId);
            var result = orderEnum.Select(order => new OrderResultModel
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                TOrderItems = order.TOrderItems.Select(item => new OrderItemResultModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                })
            });
            return result;
        }

        public async Task<bool> CreateOrderAsync(CreateOrderInfo orderInfo)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var cartItems = await _unitOfWork.CartRepository.GetItemsEnumAsync(orderInfo.MemberId);
                    if (cartItems == null) 
                    {
                        return false;
                    }
                    var orderCondition = new OrderCondition
                    {
                        MemberId = orderInfo.MemberId,
                        TotalPrice = cartItems.Sum(item =>  item.Price* item.Quantity),
                        OrderDate = DateTime.Now
                    };
                    var newOrder = await _unitOfWork.OrderRepository.CreateAsync(orderCondition);
                    await _unitOfWork.SaveChangeAsync();//關聯式資料庫需先透過SaveChanges()才能獲得識別項主鍵

                    var orderItems = cartItems.Select(item => new OrderItemCondition
                    {
                        OrderId = newOrder.OrderId,//透過SaveChanges()返回的識別項主鍵
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    });
                    await _unitOfWork.OrderRepository.CreateItemsAsync(orderItems);

                    var productsCondition = orderItems.Select(item => new ProductReduceQuantityCondition
                    {
                        ProductId = item.ProductId,
                        ReduceQuantity = item.Quantity
                    });
                    await _unitOfWork.ProductRepository.ReduceStockAsync(productsCondition);
                    await _unitOfWork.CartRepository.RemoveAllItemsAsync(orderInfo.MemberId);
                    await _unitOfWork.SaveChangeAsync();

                    var stripeInfo = new StripeInfo { TotalPrice = newOrder.TotalPrice, StripeToken = orderInfo.StripeToken };
                    _stripeService.CreateOrder(stripeInfo);

                    await transaction.CommitAsync();

                    return true;
                }
                catch
                {
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }
    }
}
