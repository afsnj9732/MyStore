using Microsoft.EntityFrameworkCore.Storage;
using MyStore.Server.Models.Repository.Interfaces;


namespace MyStore.Server.Models.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IMemberRepository MemberRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task SaveChangeAsync();
    }
}
