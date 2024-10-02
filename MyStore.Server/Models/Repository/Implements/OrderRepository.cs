using Microsoft.EntityFrameworkCore;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Repository.Interfaces;


namespace MyStore.Server.Models.Repository.Implements
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbStoreContext _db;

        public OrderRepository(DbStoreContext db)
        {
            _db = db;
        }
        public async Task CreateItemsAsync(IEnumerable<OrderItemCondition> orderItems)
        {
            var  AddItems = orderItems.Select(item=> new TOrderItem 
            {
                OrderId = item.OrderId,
                ProductId = item.ProductId,
                Quantity = item.Quantity
            });
            await _db.TOrderItems.AddRangeAsync(AddItems);
        }
        public async Task<TOrder> CreateAsync(OrderCondition condition)
        {
            var createOrder = new TOrder
            {
                MemberId = condition.MemberId,
                TotalPrice = condition.TotalPrice,
                OrderDate = condition.OrderDate
            };
            await _db.TOrders.AddAsync(createOrder);
            return createOrder;
        }


        public async Task<IEnumerable<OrderDataModel>> GetEnumAsync(int memberId)
        {
            var Orders = await _db.TOrders.Where(order => order.MemberId == memberId)
                .Include(order => order.TOrderItems)
                .ThenInclude(orderItem => orderItem.Product)
                .OrderByDescending(orderItem => orderItem.OrderDate)
                .ToListAsync();
            var result = Orders.Select(order => new OrderDataModel
            {
                TotalPrice = order.TotalPrice,
                OrderDate = order.OrderDate,
                TOrderItems = order.TOrderItems.Select(item => new OrderItemDataModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity
                })
            });
            return result;
        }
    }
}
