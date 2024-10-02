using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStore.Server.Controllers.Dtos.Parameters;
using MyStore.Server.Controllers.Dtos.ViewModels;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Interfaces;

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
            var productInfo = new ProductViewInfo
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
                })
            };

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateProductAsync(ProductParameter productParameter)
        {
            var productInfo = new ProductInfo
            {
                ProductId = productParameter.ProductId,
                Price = productParameter.Price,
                ImageUrl = productParameter.ImageUrl,
                Name = productParameter.Name,
                StockQuantity = productParameter.StockQuantity,
                Description = productParameter.Description
            };
            await _productService.UpdateProductAsync(productInfo);
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
