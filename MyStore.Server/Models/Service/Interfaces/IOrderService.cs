using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(CreateOrderInfo orderInfo);
        Task<List<OrderResultModel>> GetOrdersAsync(int memberId);
    }
}
