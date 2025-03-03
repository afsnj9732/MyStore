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
using MyStore.Server.Models.Factories.Interfaces;
using MyStore.Server.Models.Service.Implements;
using Microsoft.Extensions.Configuration;



namespace MyStore.Tests
{
    public class OrderControllerTests
    {
        //private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IPaymentService> _mockPaymentService;
        private readonly Mock<IPaymentFactory> _mockPaymentFactory;
        private readonly Mock<ICartService> _mockICartService;
        private readonly Mock<IOrderService> _mockOrderService;
        private readonly OrderController orderController;
        public OrderControllerTests()
        {
            //模擬 IConfiguration 返回 IConfigurationSection
            //var _mockStripeSection = new Mock<IConfigurationSection>();
            //_mockStripeSection.Setup(x => x["SecretKey"])
            //    .Returns("StripeKey");
            //var _mockDomainSection = new Mock<IConfigurationSection>();
            //_mockDomainSection.Setup(x => x["local"])
            //    .Returns("http://localhost");

            //_mockConfiguration = new Mock<IConfiguration>();
            //_mockConfiguration.Setup(x => x.GetSection("Stripe")).Returns(_mockStripeSection.Object);
            //_mockConfiguration.Setup(x => x.GetSection("Domain")).Returns(_mockDomainSection.Object);

            _mockPaymentService = new Mock<IPaymentService>();

            _mockOrderService = new Mock<IOrderService>();
            _mockPaymentFactory = new Mock<IPaymentFactory>();
            _mockICartService = new Mock<ICartService>();
            orderController = new OrderController(_mockOrderService.Object, _mockPaymentFactory.Object, _mockICartService.Object);
        }

        private void SetClaimsPrincipal()
        {
            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, "1")
            };
            var claimsIdentity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            orderController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
        }

        [Fact]
        public async Task OrderListAsync_ReturnOrderList()
        {
            //Arrange
            SetClaimsPrincipal();

            var fetchedOrderList = new Mock<List<OrderResultModel>>();

            _mockOrderService.Setup(orderService => orderService.GetOrdersAsync(It.IsAny<int>()))
               .ReturnsAsync(fetchedOrderList.Object);

            //Act
            var result = await orderController.OrderListAsync();
            //result是IActionResult類型，當回傳OK()的時候，是回傳200


            //Assert
            var resultObject = Assert.IsType<OkObjectResult>(result);
            //透過Assert是否為OkObjectResult，可以獲取回傳值
            Assert.IsAssignableFrom<IEnumerable<OrderViewModel>>(resultObject.Value);
        }

        [Fact]
        public async Task PlaceOrderAsync_Success()
        {
            //Arrange

            SetClaimsPrincipal();
            var placeOrderParameter = new StripeParameter { SessionId = "111", PaymentMode = "stripe" };
            _mockPaymentFactory.Setup(factory => factory.CreatePaymentService("stripe"))
                .Returns(_mockPaymentService.Object);
            //使用者選擇stripe付款的情境

            _mockPaymentService.Setup(stripeService => stripeService.CheckPayment(placeOrderParameter.SessionId))
                .Returns(true);
            //付款檢查完成的情境
            var createOrderInfo = new CreateOrderInfo { MemberId = 1 };
            var resultModel = new OrderResultModel 
            { 
                TotalPrice = 1 ,
                OrderDate = DateTime.Now,
                TOrderItems = new List<OrderItemResultModel>
                {
                    new OrderItemResultModel
                    {
                        Price = 1 ,
                        ProductId = 1 ,
                        ProductName = "111",
                        Quantity = 1 
                    }
                }
            };
            _mockOrderService.Setup(orderService => orderService.CreateOrderAsync(It.IsAny<CreateOrderInfo>()))
                .ReturnsAsync(resultModel);
                
            //Act
            var result = await orderController.PlaceOrderAsync(placeOrderParameter);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
