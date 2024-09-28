using MyStore.Server.Controllers.Dtos.Parameters;
using MyStore.Server.Controllers.Dtos.ViewModels;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyStore.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> CartItemListAsync()
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var totalPrice = await _cartService.GetTotalPriceAsync(memberId);
            var itemList = await _cartService.GetCartItemsAsync(memberId);
            if(itemList == null)
            {
                return Ok();
            }
            var cartItems = itemList.Select(item => new CartItemViewModel {
              ProductId = item.ProductId,
              ProductName = item.ProductName,
              ProductStockQuantity = item.ProductStockQuantity,
              Quantity = item.Quantity,
              Price = item.Price
            }).ToList();
            var result = new CartViewModel
            {
                TotalPrice = totalPrice,
                CartItems = cartItems
            };
            return Ok(result);
        }

        [Authorize]
        [HttpGet("count")]
        public async Task<IActionResult> GetCartItemCountAsync()
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var itemCount = await _cartService.GetCartItemCountAsync(memberId);
            return Ok(itemCount);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddToCartAsync(CartItemParameter info)
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var itemInfo = new CartItemInfo
            {
                ProductId = info.ProductId,
                Quantity = info.Quantity,
                MemberId = memberId
            };
            await _cartService.AddCartItemAsync(itemInfo);
            return Ok();
        }

        //[Authorize]
        //[HttpGet("price")]
        //public async Task<IActionResult> GetTotalPriceAsync()
        //{
        //    var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //    var result = await _cartService.GetTotalPriceAsync(memberId);
        //    return Ok(result);
        //}

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateItemAsync(CartItemParameter info)
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var itemInfo = new CartItemInfo
            {
                ProductId = info.ProductId,
                Quantity = info.Quantity,
                MemberId = memberId
            };
            await _cartService.UpdateCartItemAsync(itemInfo);
            return Ok();
        }


        [Authorize]
        [HttpPost("delete/{productId:int}")]
        public async Task<IActionResult> DeleteItemAsync([FromRoute]int productId)
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var itemInfo = new CartItemInfo
            {
                MemberId = memberId,
                ProductId = productId
            };
            await _cartService.RemoveCartItemAsync(itemInfo);
            return Ok();
        }

    }
}
