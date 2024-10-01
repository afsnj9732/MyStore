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
        public ProductRepository(DbStoreContext db)
        {
            _db = db;
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
        public async Task<IEnumerable<ProductDataModel>> GetProductEnumBySearchWordAsync(string searchWord)
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

        public async Task ReduceProductQuantityAsync(IEnumerable<ProductReduceQuantityCondition> productsCondition)
        {
            var products = await _db.TProducts.ToListAsync();
            foreach (var product in productsCondition)
            {
                var target = products.Where(p => p.ProductId == product.ProductId).FirstOrDefault();
                target.StockQuantity -= product.ReduceQuantity;
                if (target.StockQuantity < 0)
                {
                    throw new Exception();
                }
            }

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
