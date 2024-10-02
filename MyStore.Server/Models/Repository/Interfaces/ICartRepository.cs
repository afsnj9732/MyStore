using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;

namespace MyStore.Server.Models.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task AddItemAsync(CartItemCondition cartItem);
        Task RemoveItemAsync(CartItemCondition cartItem);
        Task UpdateItemAsync(CartItemCondition cartItem);
        Task CreateAsync(int memberId);
        Task<int> GetIdAsync(int memberId);
        Task RemoveAllItemsAsync(int memberId);
        Task<IEnumerable<CartItemDataModel>> GetItemsEnumAsync(int id);
    }
}
