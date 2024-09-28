using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.UnitOfWork;
using Stripe;


namespace MyStore.Server.Models.Service.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<OrderResultModel>> GetOrdersAsync(int memberId)
        {
            var orderEnum = await _unitOfWork.OrderRepository.GetOrderEnumAsync(memberId);
            var result = orderEnum.Select(order => new OrderResultModel
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                TOrderItems = order.TOrderItems.Select(item => new OrderItemResultModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                }).ToList()
            }).ToList();
            return result;
        }

        public async Task<bool> CreateOrderAsync(CreateOrderInfo orderInfo)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var cartItems = await _unitOfWork.CartRepository.GetCartItemsEnumByUserIdAsync(orderInfo.MemberId);
                    var orderCondition = new OrderCondition
                    {
                        MemberId = orderInfo.MemberId,
                        TotalPrice = cartItems.Sum(item =>  item.Price* item.Quantity),
                        OrderDate = DateTime.Now
                    };
                    var newOrder = await _unitOfWork.OrderRepository.CreateOrderAsync(orderCondition);
                    await _unitOfWork.Save();//關聯式資料庫需先透過SaveChanges()才能獲得識別項主鍵

                    var orderItems = cartItems.Select(item => new OrderItemCondition
                    {
                        OrderId = newOrder.OrderId,//透過SaveChanges()返回的識別項主鍵
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    });
                    await _unitOfWork.OrderRepository.CreateOrderItemAsync(orderItems);
                    await _unitOfWork.CartRepository.RemoveUserAllCartItemsAsync(orderInfo.MemberId);
                    await _unitOfWork.Save();

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
