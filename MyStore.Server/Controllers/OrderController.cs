using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyStore.Server.Controllers.Dtos.Parameters;
using MyStore.Server.Controllers.Dtos.ViewModels;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Interfaces;
using System.Security.Claims;

namespace MyStore.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IStripeService _stripeService;
        private readonly ICartService _cartService;

        public OrderController(IOrderService orderService,IStripeService stripeService, ICartService cartService)
        {
            _orderService = orderService;
            _stripeService = stripeService;
            _cartService = cartService;
        }
        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> OrderListAsync()
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orderList = await _orderService.GetOrdersAsync(memberId);
            var result = orderList.Select(order => new OrderViewModel
            {
               OrderDate = order.OrderDate,
               TotalPrice = order.TotalPrice,
                TOrderItems = order.TOrderItems.Select(item => new OrderItemViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                })
            });
            return Ok(result);
        }

        [Authorize]
        [HttpPost("fullfill")]
        public async Task<IActionResult> PlaceOrderAsync(StripeParameter stripeParameter)
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var isGetPayment = await _stripeService.CheckPayment(stripeParameter.SessionId);
            if (!isGetPayment) { return BadRequest("付款未完成"); }

            var orderInfo = new CreateOrderInfo { MemberId = memberId };
            var orderDetail = await _orderService.CreateOrderAsync(orderInfo);
       
            if (orderDetail!=null)
            {
                //回傳訂單資料
                return Ok(orderDetail);
            }
            return BadRequest("訂購失敗，請確認庫存或連線");
        }
    }


}
