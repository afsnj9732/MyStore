using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;

namespace MyStore.Server.Models.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task AddCartItemAsync(CartItemCondition cartItem);
        Task RemoveCartItemAsync(CartItemCondition cartItem);
        Task UpdateCartItemAsync(CartItemCondition cartItem);
        Task CreateCartAsync(int memberId);
        Task<int> GetCartIdAsync(int memberId);
        Task RemoveUserAllCartItemsAsync(int memberId);
        Task<IEnumerable<CartItemDataModel>> GetCartItemsEnumByUserIdAsync(int id);
    }
}
