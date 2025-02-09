using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResultModel> CreateOrderAsync(CreateOrderInfo orderInfo);
        Task<IEnumerable<OrderResultModel>> GetOrdersAsync(int memberId);
    }
}
