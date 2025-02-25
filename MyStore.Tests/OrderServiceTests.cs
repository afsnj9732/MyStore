using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Service.Dtos.Infos;
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
        private readonly Mock<ILogger<OrderService>> _mockLogger;
        private readonly IOrderService orderService;
        private readonly Mock<IDbContextTransaction> _mockTransaction;
        public OrderServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockStripeService = new Mock<IStripeService>();
            _mockTransaction = new Mock<IDbContextTransaction>();
            _mockLogger = new Mock<ILogger<OrderService>>();
            orderService = new OrderService(_mockUnitOfWork.Object, _mockStripeService.Object,_mockLogger.Object);
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


        [Fact]
        public async Task CreateOrderAsync_ByOrderInfo_ReturnBool()
        {
            //Arrange
            var orderInfo = new CreateOrderInfo
            {
                MemberId = 1,
                StripeToken = "StripeToken"
            };
            var cartItemsData = new List<CartItemDataModel>
            {
                new CartItemDataModel
                {
                    Price = 100,
                    ProductId = 1,
                    ProductName = "ProductName",
                    Quantity = 5 ,
                    ProductStockQuantity = 10,
                }
            };

            //var orderCondition = new OrderCondition
            //{
            //    MemberId = orderInfo.MemberId,
            //    TotalPrice = cartItemsData.Sum(item => item.Price * item.Quantity),
            //    OrderDate = DateTime.Now
            //};
            var createdNewOrder = new TOrder
            {
                MemberId = 1,
                OrderDate = DateTime.Now,
                OrderId = 1,
                TotalPrice = 100
            };
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.BeginTransactionAsync())
                .ReturnsAsync(_mockTransaction.Object); //.Object用以獲取介面實例

            var cartItems =  _mockUnitOfWork.Setup(unitOfWork => unitOfWork.CartRepository.GetItemsEnumAsync(orderInfo.MemberId))
                .ReturnsAsync(cartItemsData);

            var newOrder =  _mockUnitOfWork.Setup(unitOfWork=>unitOfWork.OrderRepository.CreateAsync(It.IsAny<OrderCondition>()))
                .ReturnsAsync(createdNewOrder);


             _mockUnitOfWork.Setup(unitOfWork=>unitOfWork.SaveChangeAsync())
                .Returns(Task.CompletedTask)
                .Verifiable();
                                             //Task.CompletedTask表示void的非同步方法完成
                                             //同步方法可以使用Verifiable()來確認是否被調用
                                             //因為方法不具備回傳值，因此沒有ReturnsAsync可以調用
            _mockUnitOfWork.Setup(unitOfWork=>unitOfWork.OrderRepository.CreateItemsAsync(It.IsAny<List<OrderItemCondition>>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(unitOfWork=>unitOfWork.ProductRepository.ReduceStockAsync(It.IsAny<IEnumerable<ProductReduceQuantityCondition>>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.CartRepository.RemoveAllItemsAsync(orderInfo.MemberId))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangeAsync())
                .Returns(Task.CompletedTask).Verifiable();

            _mockStripeService.Setup(unitOfWork => unitOfWork.CreateOrder(It.IsAny<PaymentInfo>()))
                .Verifiable();

            _mockTransaction.Setup(unitOfWork => unitOfWork.CommitAsync(default));

            //Act
            var result = await orderService.CreateOrderAsync(orderInfo);

            //Assert
            Assert.True(result);
            _mockStripeService.Verify(unitOfWork => unitOfWork.CreateOrder(It.IsAny<PaymentInfo>()), Times.Once);
            _mockUnitOfWork.Verify(unitOfWork => unitOfWork.SaveChangeAsync(),Times.Exactly(2));//調用2次，若不符則失敗


        }

    }
}
