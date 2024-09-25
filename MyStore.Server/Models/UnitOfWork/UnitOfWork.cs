using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Interfaces;
using MyStore.Server.Models.Service.Implements;
using MyStore.Server.Models.Service.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;


namespace MyStore.Server.Models.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbStoreContext _db;
        private readonly IProductRepository _productRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;

        public UnitOfWork(DbStoreContext db, IProductRepository productRepository, IMemberRepository memberRepository, ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            _db = db;
            _cartRepository = cartRepository;
            _orderRepository = orderRepository;
            _memberRepository = memberRepository;
           _productRepository = productRepository;

        }

        public IProductRepository ProductRepository => _productRepository;

        public IMemberRepository MemberRepository => _memberRepository;

        public ICartRepository CartRepository => _cartRepository;

        public IOrderRepository OrderRepository => _orderRepository;
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _db.Database.BeginTransactionAsync();
        }
        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
