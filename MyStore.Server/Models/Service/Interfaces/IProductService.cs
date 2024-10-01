using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface IProductService
    {
        Task<ProductsResultModel> GetCurrentPageProductAsync(ProductInfo productInfo);
        Task<ProductResultModel?> GetProductDetailByIdAsync(int id);
    }
}
