using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;

namespace MyStore.Server.Models.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task UpdateAsync(ProductCondition productCondition);
        Task<IEnumerable<ProductDataModel>> GetProductEnumBySearchWordAsync(string searchWord);
        Task<ProductDataModel?> GetProductByIdAsync(int productId);
        Task ReduceProductQuantityAsync(List<ProductReduceQuantityCondition> productsCondition);
    }
}
