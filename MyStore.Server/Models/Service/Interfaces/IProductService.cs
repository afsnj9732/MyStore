using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface IProductService
    {
        Task UpdateProductAsync(ProductInfo productCondition);
        Task<ProductsResultModel> GetCurrentPageProductAsync(ProductViewInfo productInfo);
        Task<ProductResultModel?> GetProductDetailByIdAsync(int id);
    }
}
