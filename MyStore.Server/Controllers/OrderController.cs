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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;

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
                }).ToList()
            });
            return Ok(result);
        }

        [Authorize]
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrderAsync([FromRoute]string stripeToken)
        {
            var memberId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orderInfo = new CreateOrderInfo { MemberId = memberId,StripeToken = stripeToken };
            var isSuccess = await _orderService.CreateOrderAsync(orderInfo);
            if (isSuccess)
            {
                return RedirectToAction("OrderList");
            }
            return RedirectToAction("CartItemList", "Cart");        
        }
    }

    
}
