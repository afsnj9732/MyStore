using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Repository.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStore.Tests
{
    public class OrderRepositoryTests
    {
        private readonly Mock<DbStoreContext> _mockDbStoreContext;
        private readonly Mock<DbSet<TOrder>> _mockTOrderDbSet;
        private readonly Mock<DbSet<TOrderItem>> _mockTOrderItemDbSet;
        private readonly OrderRepository orderRepository;

        private readonly DbStoreContext _db;
        private readonly OrderRepository _orderRepository;
        public OrderRepositoryTests() 
        { 
            _mockDbStoreContext = new Mock<DbStoreContext>();
            _mockTOrderDbSet = new Mock<DbSet<TOrder>>();
            _mockTOrderItemDbSet = new Mock<DbSet<TOrderItem>>();
            _mockDbStoreContext.Setup(dbContext=>dbContext.TOrders).Returns(_mockTOrderDbSet.Object);
            _mockDbStoreContext.Setup(dbContext=>dbContext.TOrderItems).Returns(_mockTOrderItemDbSet.Object);
            orderRepository = new OrderRepository(_mockDbStoreContext.Object);

            //Entity Framework In-Memory
            var options = new DbContextOptionsBuilder<DbStoreContext>()
                          .UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            _db = new DbStoreContext(options);
            _orderRepository = new OrderRepository(_db);
        }

        [Fact]
        public async Task CreateItemsAsync_ByOrderItemConditions()
        {
            //Arrange
            var orderItems = new List<OrderItemCondition>
            {
                new OrderItemCondition
                {
                    OrderId = 1,
                    ProductId = 1,
                    Quantity = 1
                }
            };
            //Act
            await orderRepository.CreateItemsAsync(orderItems);
            //Assert
            _mockTOrderItemDbSet.Verify(dbSet=>dbSet.AddRangeAsync(It.IsAny<IEnumerable<TOrderItem>>(),default),Times.Once);

        }

        [Fact]
        public async Task GetEnumAsync_ByMemberId_ReturnOrderDataModels()
        {
            //Arrange
            var memberId = 1;
            var orderDatas = new List<TOrder>
            {
                new TOrder
                {
                    TotalPrice = 100,
                    MemberId = 1,
                    OrderDate = DateTime.Now,
                    OrderId = 1,
                    TOrderItems = new List<TOrderItem>
                    {
                        new TOrderItem
                        {
                            ProductId = 1,
                            Quantity = 1,
                            Product = new TProduct
                            {
                                Name = "Test"
                            }
                        }
                    }
                }
            };

            await _db.TOrders.AddRangeAsync(orderDatas);
            await _db.SaveChangesAsync();

            //_mockTOrderDbSet.As<IQueryable<TOrder>>().Setup(dbSet => dbSet.Provider).Returns(orderDatas.Provider);
            //_mockTOrderDbSet.As<IQueryable<TOrder>>().Setup(dbSet => dbSet.Expression).Returns(orderDatas.Expression);
            //_mockTOrderDbSet.As<IQueryable<TOrder>>().Setup(dbSet => dbSet.ElementType).Returns(orderDatas.ElementType);
            //_mockTOrderDbSet.As<IQueryable<TOrder>>().Setup(dbSet => dbSet.GetEnumerator()).Returns(orderDatas.GetEnumerator());

            //Act
            var result = await _orderRepository.GetEnumAsync(memberId);
            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<OrderDataModel>>(result);

        }
    }
}
