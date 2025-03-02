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
        private readonly IOrderService orderService;//用來建立真實的orderService

        //用Moq套件，模擬Interface的結構，建立符合Interface規範的假類別
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IDbContextTransaction> _mockTransaction;
        private readonly Mock<ILogger<OrderService>> _mockLogger;
        public OrderServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockTransaction = new Mock<IDbContextTransaction>();
            _mockLogger = new Mock<ILogger<OrderService>>();
            orderService = new OrderService(_mockUnitOfWork.Object,_mockLogger.Object);
            //.Object的用意在於取得實例
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnOrderResultModels()
        {
            //Arrange
            int mockMemberId = It.IsAny<int>();
            var mockOrderList = new Mock<List<OrderDataModel>>();
            //若不操作List，直接用Mock模擬即可

            _mockUnitOfWork.Setup(unitOfWork => 
                 unitOfWork.OrderRepository.GetEnumAsync(mockMemberId))
                 .ReturnsAsync(mockOrderList.Object);
            //這個單元的行為在於，將mockOrderList的型別轉換成OrderResultModel
            //因此並不關注OrderList的具體資料

            //Act
            var result = await orderService.GetOrdersAsync(mockMemberId);


            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<OrderResultModel>>(result);

        }


        [Fact]
        public async Task CreateOrderAsync_ByOrderInfo_SuccessAndFail()
        {
            //Arrange
            int memberId = It.IsAny<int>();
            var orderInfo = new Mock<CreateOrderInfo>();
            var cartItemsData = new Mock<List<CartItemDataModel>>();
            var createdNewOrder = new Mock<TOrder>();

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.BeginTransactionAsync())
                .ReturnsAsync(_mockTransaction.Object);

            var cartItems = _mockUnitOfWork.Setup(unitOfWork => unitOfWork.CartRepository.GetItemsEnumAsync(memberId))
                .ReturnsAsync(cartItemsData.Object);

            var newOrder = _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrderRepository.CreateAsync(It.IsAny<OrderCondition>()))
                .ReturnsAsync(createdNewOrder.Object);

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangeAsync())
               .Returns(Task.CompletedTask)
               .Verifiable();
            //因為方法不具備回傳值，因此沒有ReturnsAsync可以調用
            //Task.CompletedTask表示void的非同步方法完成
            //非同步方法可以使用Verifiable()來確認是否被調用

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrderRepository.CreateItemsAsync(It.IsAny<List<OrderItemCondition>>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.ProductRepository.ReduceStockAsync(It.IsAny<IEnumerable<ProductReduceQuantityCondition>>()))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.CartRepository.RemoveAllItemsAsync(memberId))
                .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.SaveChangeAsync())
                .Returns(Task.CompletedTask).Verifiable();


            _mockTransaction.Setup(unitOfWork => unitOfWork.CommitAsync(default))
                .Returns(Task.CompletedTask).Verifiable();

            //Act(Success)
            var successResult = await orderService.CreateOrderAsync(orderInfo.Object);

            //Assert(Success)
            Assert.IsAssignableFrom<OrderResultModel>(successResult);
            _mockTransaction.Verify(transaction => transaction.CommitAsync(default), Times.Once);//調用1次，若不符則失敗
            _mockUnitOfWork.Verify(unitOfWork => unitOfWork.SaveChangeAsync(), Times.Exactly(2));//調用2次，若不符則失敗

            //Arrange(Fail)
            _mockUnitOfWork.Setup(unitOfWork => unitOfWork.OrderRepository.CreateAsync(It.IsAny<OrderCondition>()))
                   .ThrowsAsync(new Exception());
            //主動拋出錯誤，模擬失敗情形

            //Act(Fail)
            await orderService.CreateOrderAsync(orderInfo.Object);


            //Assert(Fail)
            _mockTransaction.Verify(transaction => transaction.RollbackAsync(default), Times.Once);
            //檢查Rollback是否被調用

            //_mockTransaction.Verify(transaction => transaction.CommitAsync(default), Times.Exactly(2));
            //經測試Verify驗證的範圍是整個function  所以要把成功時調用的次數也算在內
        }

    }
}
