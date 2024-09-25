using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface ICartService
    {
        Task<List<CartResultModel>?> GetCartItemsAsync(int id);
        Task<int> GetTotalPriceAsync(int memberId);
        Task<int> GetCartItemCountAsync(int id);

        Task AddCartItemAsync(CartItemInfo cartItem);

        Task UpdateCartItemAsync(CartItemInfo cartItem);
        Task RemoveCartItemAsync(CartItemInfo cartItem);
    }
}
