﻿using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.UnitOfWork;


namespace MyStore.Server.Models.Service.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IUnitOfWork unitOfWork, ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderResultModel>> GetOrdersAsync(int memberId)
        {
            var orderEnum = await _unitOfWork.OrderRepository.GetEnumAsync(memberId);
            var result = orderEnum.Select(order => new OrderResultModel
            {
                OrderDate = order.OrderDate,
                TotalPrice = order.TotalPrice,
                TOrderItems = order.TOrderItems.Select(item => new OrderItemResultModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity
                })
            });
            return result;
        }

        public async Task<OrderResultModel> CreateOrderAsync(CreateOrderInfo orderInfo)
        {
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var cartItems = await _unitOfWork.CartRepository.GetItemsEnumAsync(orderInfo.MemberId);
                    if (cartItems == null) 
                    {
                        _logger.LogWarning("購物車為空");
                        return null;
                    }
                    var orderCondition = new OrderCondition
                    {
                        MemberId = orderInfo.MemberId,
                        TotalPrice = cartItems.Sum(item =>  item.Price* item.Quantity),
                        OrderDate = DateTime.Now
                    };
                    var newOrder = await _unitOfWork.OrderRepository.CreateAsync(orderCondition);
                    await _unitOfWork.SaveChangeAsync();//關聯式資料庫需先透過SaveChanges()才能獲得識別項主鍵




                    var orderItems = cartItems.Select(item => new OrderItemCondition
                    {
                        OrderId = newOrder.OrderId,//透過SaveChanges()返回的識別項主鍵
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                    });
                    await _unitOfWork.OrderRepository.CreateItemsAsync(orderItems);

                    var productsCondition = orderItems.Select(item => new ProductReduceQuantityCondition
                    {
                        ProductId = item.ProductId,
                        ReduceQuantity = item.Quantity
                    });
                    await _unitOfWork.ProductRepository.ReduceStockAsync(productsCondition);
                    await _unitOfWork.CartRepository.RemoveAllItemsAsync(orderInfo.MemberId);
                    await _unitOfWork.SaveChangeAsync();

                    await transaction.CommitAsync();

                    var latestOrder = new OrderResultModel()
                    {
                        OrderDate = newOrder.OrderDate,
                        TotalPrice = newOrder.TotalPrice,
                        TOrderItems = cartItems.Select(item => new OrderItemResultModel
                        {
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            Quantity = item.Quantity,
                            Price = item.Price
                        })
                    };

                    return latestOrder;
                }
                catch(Exception ex)
                {
                    _logger.LogError("訂單建立錯誤,參考訊息:{ Message }",ex.Message);
                    await transaction.RollbackAsync();
                    return null;
                }
            }
        }
    }
}
