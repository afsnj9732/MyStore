using MyStore.Server.Controllers.Dtos.ViewModels;
using MyStore.Server.Models.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        //[HttpGet("count")]
        //public async Task<IActionResult> GetProductTotalPageAsync()
        //{
        //    var result = await _productService.GetProductTotalPageAsync();
        //    return Ok(result);
        //}

        [HttpGet("list/{page:int}")]
        public async Task<IActionResult> ProductListAsync([FromRoute] int page = 1)
        {
            var products = await _productService.GetCurrentPageProductAsync(page);
            var totalPage = await _productService.GetProductTotalPageAsync();
            var productViewModel = products.Select(product => new ProductViewModel
            {
                Description = product.Description,
                ProductId = product.ProductId,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                StockQuantity = product.StockQuantity,
            });
            var result = new ProductListViewModel
            {
                TotalPage = totalPage,
                Products = productViewModel
            };
            return Ok(result);
        }
        [HttpGet("list/detail/{productId:int}")]
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
