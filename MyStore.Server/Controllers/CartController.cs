using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStore.Server.Controllers.Dtos.Parameters;
using MyStore.Server.Controllers.Dtos.ViewModels;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Implements;
using MyStore.Server.Models.Service.Interfaces;
using System.Security.Claims;

namespace MyStore.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IStripeService _stripeService;
        public CartController(ICartService cartService, IStripeService stripeService)
        {
            _cartService = cartService;
            _stripeService = stripeService;
        }

        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> CartItemListAsync()
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var totalPrice = await _cartService.GetCartTotalPriceAsync(memberId);
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
            });
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
            var itemCount = await _cartService.GetCartItemsCountAsync(memberId);
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

        [Authorize]
        [HttpPost("stripe")]
        public async Task<IActionResult> CallStripeCheckoutAsync()
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cartItems = await _cartService.CallStripeCheckOut(memberId);
            //建立Stripe頁面
            var stripeInfo = new StripeInfo { CartItems = cartItems };
            var stripeUrl =  _stripeService.CreateOrder(stripeInfo);
            if (stripeUrl != null)
            {
                return Ok(stripeUrl);
                //Response.Headers.Add("Location", stripeUrl);
                //return new StatusCodeResult(303);
            }
            return BadRequest();

        }

    }
}
