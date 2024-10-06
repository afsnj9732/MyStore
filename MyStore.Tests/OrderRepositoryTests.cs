using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
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
         public OrderRepositoryTests() 
        { 
            _mockDbStoreContext = new Mock<DbStoreContext>();
            _mockTOrderDbSet = new Mock<DbSet<TOrder>>();
            _mockTOrderItemDbSet = new Mock<DbSet<TOrderItem>>();
            _mockDbStoreContext.Setup(dbContext=>dbContext.TOrders).Returns(_mockTOrderDbSet.Object);
            _mockDbStoreContext.Setup(dbContext=>dbContext.TOrderItems).Returns(_mockTOrderItemDbSet.Object);
            orderRepository = new OrderRepository(_mockDbStoreContext.Object);
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


    }
}
