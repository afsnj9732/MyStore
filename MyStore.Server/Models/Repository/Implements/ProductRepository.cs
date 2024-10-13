using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Repository.Interfaces;

namespace MyStore.Server.Models.Repository.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly DbStoreContext _db;
        private readonly ILogger<ProductRepository> _logger;
        public ProductRepository(DbStoreContext db, ILogger<ProductRepository> logger)
        {
            _db = db;
            _logger = logger;
        }
        public async Task UpdateAsync(ProductCondition productCondition)
        {
            var target = await _db.TProducts.Where(product=>product.ProductId==productCondition.ProductId).FirstOrDefaultAsync();
            if (target != null) {
                target.Price = productCondition.Price ?? target.Price;
                target.Name = productCondition.Name ?? target.Name;
                target.Description = productCondition.Description ?? target.Description;
                target.StockQuantity = productCondition.StockQuantity ?? target.StockQuantity;
                target.ImageUrl = productCondition.ImageUrl ?? target.ImageUrl;
            }
        } 
        public async Task<IEnumerable<ProductDataModel>> GetEnumAsync(string searchWord)
        {
            var products = await _db.TProducts.FromSqlRaw("EXEC usp_GetAllProducts @SearchWord",
                new SqlParameter("SearchWord", searchWord))
                .ToListAsync();

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

        public async Task ReduceStockAsync(IEnumerable<ProductReduceQuantityCondition> productsCondition)
        {
            var products = await _db.TProducts.ToListAsync();
            foreach (var product in productsCondition)
            {
                var target = products.Where(p => p.ProductId == product.ProductId).FirstOrDefault();
                if(target == null)
                {
                    _logger.LogWarning("找不到對應的商品,商品ID:{ProductId}",product.ProductId);
                    continue;
                }
                target.StockQuantity -= product.ReduceQuantity;
                if (target.StockQuantity < 0)
                {

                    _logger.LogError("庫存不足, 商品ID: {ProductId}", product.ProductId);
                    throw new InvalidOperationException("庫存不足");
                }
            }

        }

        public async Task<ProductDataModel?> GetAsync(int productId)
        {
            var product = await _db.TProducts.Where(product => product.ProductId == productId).FirstOrDefaultAsync();

            if (product == null) 
            { 
                return null; 
            }

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
