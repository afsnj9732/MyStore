using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;

namespace MyStore.Server.Models.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDataModel>> GetOrderEnumAsync(int memberId);
        Task CreateOrderItemAsync(IEnumerable<OrderItemCondition> orderItems);
        Task<TOrder> CreateOrderAsync(OrderCondition condition);
    }
}
