using MyStore.Server.Controllers;
using MyStore.Server.Models.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MyStore.Server.Controllers.Dtos.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using MyStore.Server.Controllers.Dtos.Parameters;
using MyStore.Server.Models.Service.Dtos.Infos;

namespace MyStore.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrderController orderController;
        public OrderControllerTests()
        {
            _mockOrderService = new Mock<IOrderService>();
            orderController = new OrderController(_mockOrderService.Object);
        }

        [Fact]  
        public async Task OrderListAsync_ReturnOrderList()
        {
            //Arrange
            var memberId = 1;
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, memberId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            orderController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            var fetchedOrderList = new List<OrderResultModel> 
            {
                new OrderResultModel
                {
                    OrderDate = DateTime.Now,
                    TotalPrice = 100,
                    TOrderItems = new List<OrderItemResultModel>
                    {
                        new OrderItemResultModel
                        {
                          ProductId = 1,
                          ProductName = "Test",
                          Quantity = 1
                        }
                    }
                }
            };
             _mockOrderService.Setup(orderService => orderService.GetOrdersAsync(memberId))
                .ReturnsAsync(fetchedOrderList);
            //Act
            var result = await orderController.OrderListAsync();

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            //OkResult只回傳200，OkObjectResult回傳200和回傳值
            var okValue = Assert.IsAssignableFrom<IEnumerable<OrderViewModel>>(okObjectResult.Value);
            Assert.NotEmpty(okValue);
        }

        [Fact]
        public async Task PlaceOrderAsync_Success()
        {
            //Arrange
            var memberId = 1;
            var placeOrderParameter = new PlaceOrderParameter { StripeToken = "123" };
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, memberId.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            orderController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            _mockOrderService.Setup(orderService => orderService.CreateOrderAsync(It.IsAny<CreateOrderInfo>()))
                .ReturnsAsync(true);
            //Act
            var result = await orderController.PlaceOrderAsync(placeOrderParameter);
            //Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
