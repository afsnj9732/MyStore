using MyStore.Server.Models.Service.Dtos.ResultModels;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.UnitOfWork;

namespace MyStore.Server.Models.Service.Implements
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> GetProductTotalPageAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllProductEnumAsync();
            var result = (int)Math.Ceiling(products.Count() / 5.0);
            return result;

        }
        public async Task<List<ProductResultModel>> GetCurrentPageProductAsync(int page)
        {
            var products = await _unitOfWork.ProductRepository.GetAllProductEnumAsync();
            var productList = products.Skip((page - 1) * 5).Take(5);
            var result =
                productList.Select(product => new ProductResultModel
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Description = product.Description,
                    StockQuantity = product.StockQuantity
                }
            ).ToList();

            return result;
        }

        public async Task<ProductResultModel?> GetProductDetailByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return null;
            }
            var result = new ProductResultModel()
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                StockQuantity = product.StockQuantity
            };

            return result;
        }
    }
}
