using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.DataModels;

namespace MyStore.Server.Models.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDataModel>> GetAllProductEnumAsync();
        Task<ProductDataModel?> GetProductByIdAsync(int productId);
    }
}
