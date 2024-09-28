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
            var itemList = await _cartService.GetCartItemsAsync(memberId);
            if(itemList == null)
            {
                return Ok();
            }
            var result = itemList.Select(item => new CartViewModel {
              CartId = item.CartId,
              ProductId = item.ProductId,
              ProductName = item.ProductName,
              Quantity = item.Quantity,
              Price = item.Price
            });
            return Ok(result);
        }

        [Authorize]
        [HttpGet("count")]
        public async Task<IActionResult> GetCartItemCountAsync()
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var itemCount = await _cartService.GetCartItemCountAsync(memberId);
            var result = new
            {
                cartItemCount = itemCount
            };
            return Ok(result);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
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

        [Authorize]
        [HttpGet("price")]
        public async Task<IActionResult> GetTotalPriceAsync()
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _cartService.GetTotalPriceAsync(memberId);
            return Ok(result);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
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
        [ValidateAntiForgeryToken]
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteItemAsync(int productId)
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
