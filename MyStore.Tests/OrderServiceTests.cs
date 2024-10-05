using Moq;
using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using MyStore.Server.Models.Service.Implements;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Tests
{
    public class OrderServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IStripeService> _mockStripeService;
        private readonly IOrderService orderService;
        public OrderServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockStripeService = new Mock<IStripeService>();
            orderService = new OrderService(_mockUnitOfWork.Object, _mockStripeService.Object);
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnOrderResultModels()
        {
            //Arrange
            int mockMemberId = 1;
            List<OrderDataModel> mockOrderList = new List<OrderDataModel>
            {
                new OrderDataModel
                {              
                    TotalPrice =100,
                    OrderDate = DateTime.Now,
                    TOrderItems = new List<OrderItemDataModel>
                    {
                        new OrderItemDataModel
                        {
                            ProductId=1,
                            ProductName="orange",
                            Quantity = 10
                        }

                    }
                }
            };
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrderRepository.GetEnumAsync(mockMemberId)).ReturnsAsync(mockOrderList);


            //Act
            var result = await orderService.GetOrdersAsync(mockMemberId);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(mockOrderList[0].TOrderItems.ToList()[0].ProductId, result.First().TOrderItems.First().ProductId);
            Assert.IsAssignableFrom<IEnumerable<OrderResultModel>>(result);
        }


    }
}
