using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartResultModel>?> GetCartItemsAsync(int id);
        Task<int> GetCartTotalPriceAsync(int memberId);
        Task<int> GetCartItemsCountAsync(int id);

        Task AddCartItemAsync(CartItemInfo cartItem);

        Task UpdateCartItemAsync(CartItemInfo cartItem);
        Task RemoveCartItemAsync(CartItemInfo cartItem);
    }
}
