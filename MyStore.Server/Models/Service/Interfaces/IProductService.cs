using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Service.Dtos.ResultModels;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface IProductService
    {
        Task<int> GetProductTotalPageAsync();
        Task<List<ProductResultModel>> GetCurrentPageProductAsync(int page);
        Task<ProductResultModel?> GetProductDetailByIdAsync(int id);
    }
}
