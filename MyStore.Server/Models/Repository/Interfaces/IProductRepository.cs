using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;

namespace MyStore.Server.Models.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task UpdateAsync(ProductCondition productCondition);
        Task<IEnumerable<ProductDataModel>> GetEnumAsync(string searchWord);
        Task<ProductDataModel?> GetAsync(int productId);
        Task ReduceStockAsync(IEnumerable<ProductReduceQuantityCondition> productsCondition);
    }
}
