using MyStore.Server.Controllers.Dtos.ViewModels;
using MyStore.Server.Models.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MyStore.Server.Controllers.Dtos.Parameters;
using MyStore.Server.Models.Service.Dtos.Infos;

namespace MyStore.Server.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ProductListAsync([FromQuery]ProductViewParameter productParameter)
        {
            var productInfo = new ProductInfo
            {
                Page = productParameter.Page,
                SearchWord = productParameter.SearchWord
            };
            var products = await _productService.GetCurrentPageProductAsync(productInfo);
            var result =  new ProductsViewModel
            {
                TotalPage = products.TotalPage,
                Products = products.Products.Select(product=>new ProductViewModel
                {
                    Description = product.Description,
                    ProductId = product.ProductId,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    Name = product.Name,
                    StockQuantity = product.StockQuantity,
                }).ToList()
            };

            return Ok(result);
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProduct(ProductParameter productParameter)
        {

            return Ok();
        }

        [HttpGet("detail/{productId:int}")]
        public async Task<IActionResult> ProductDetailAsync([FromRoute]int productId)
        {
            var product = await _productService.GetProductDetailByIdAsync(productId);
            if (product == null) { return RedirectToAction("ProductList"); }
            var result = new ProductViewModel
            {
                Description = product.Description,
                ProductId = product.ProductId,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                StockQuantity = product.StockQuantity
            };
            return Ok(result);
        }
    }
}
