using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MyStore.Server.Models.Repository.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbStoreContext _db;
        public ProductRepository(DbStoreContext db)
        {
            _db = db;
        }
        public async Task<IEnumerable<ProductDataModel>> GetAllProductEnumAsync()
        {
            var products = await _db.TProducts.FromSqlRaw("EXEC usp_GetAllProducts").ToListAsync();
            var result = products.Select(product => new ProductDataModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
            });
            return result;
        }

        public async Task<ProductDataModel?> GetProductByIdAsync(int productId)
        {
            var product = await _db.TProducts.Where(product => product.ProductId == productId).FirstOrDefaultAsync();

            if (product == null) { return null; }

            var result = new ProductDataModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
            };
            return result;
        }
    }
}
