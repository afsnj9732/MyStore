using MyStore.Server.Models.Repository.Interfaces;
using MyStore.Server.Models.Service.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;


namespace MyStore.Server.Models.UnitOfWork
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        IMemberRepository MemberRepository { get; }
        ICartRepository CartRepository { get; }
        IOrderRepository OrderRepository { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task Save();
    }
}
