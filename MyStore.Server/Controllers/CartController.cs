using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStore.Server.Controllers.Dtos.Parameters;
using MyStore.Server.Controllers.Dtos.ViewModels;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Factories;
using MyStore.Server.Models.Factories.Interfaces;
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
        private readonly IPaymentFactory _paymentFactory;
        public CartController(ICartService cartService, IPaymentFactory paymentFactory)
        {
            _cartService = cartService;
            _paymentFactory = paymentFactory;
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
        public async Task<IActionResult> CallStripeCheckoutAsync(PaymentParameter paymentParameter)
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cartItems = await _cartService.GetCartItemsAsync(memberId);
            if(cartItems == null) { return BadRequest("購物車為空"); }
            var _paymentService = _paymentFactory.CreatePaymentService(paymentParameter.PaymentMode);
            //建立Stripe頁面
            var stripeInfo = new StripeInfo { CartItems = cartItems };
            var stripeUrl = await _paymentService.CreateStripeAsync(stripeInfo);
            if (stripeUrl != null)
            {
                return Ok(stripeUrl);
            }
            return BadRequest();

        }

    }
}
