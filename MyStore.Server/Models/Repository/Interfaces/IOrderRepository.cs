using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;

namespace MyStore.Server.Models.Repository.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderDataModel>> GetEnumAsync(int memberId);
        Task CreateItemsAsync(IEnumerable<OrderItemCondition> orderItems);
        Task<TOrder> CreateAsync(OrderCondition condition);
    }
}
