using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.UnitOfWork;


namespace MyStore.Server.Models.Service.Implements
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetCartTotalPriceAsync(int memberId)
        {
            var cartItemsList = await _unitOfWork.CartRepository.GetItemsEnumAsync(memberId);
            var result = cartItemsList?
                .Sum(item=>item.Price*item.Quantity) ?? 0;
            return result;
        }
        public async Task<IEnumerable<CartResultModel>?> GetCartItemsAsync(int memberId)
        {
            var cartItemsList = await _unitOfWork.CartRepository.GetItemsEnumAsync(memberId);
            var result = cartItemsList?.Select(item => new CartResultModel
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductStockQuantity = item.ProductStockQuantity,
                Quantity = item.Quantity,
                Price = item.Price
            });
            return result;
        }

        public async Task<int> GetCartItemsCountAsync(int memberId)
        {
            var CartItems = await _unitOfWork.CartRepository.GetItemsEnumAsync(memberId);
            var result = CartItems?.Sum(item => item.Quantity) ?? 0;
            return result;
        }

        public async Task AddCartItemAsync(CartItemInfo cartItem)
        {
            var cartId = await _unitOfWork.CartRepository.GetIdAsync(cartItem.MemberId);
            var condition = new CartItemCondition
            {
                CartId = cartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };
            await _unitOfWork.CartRepository.AddItemAsync(condition);
            await _unitOfWork.SaveChangeAsync();
        }
        public async Task UpdateCartItemAsync(CartItemInfo cartItem)
        {
            var cartId = await _unitOfWork.CartRepository.GetIdAsync(cartItem.MemberId);
            var condition = new CartItemCondition
            {
                CartId = cartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };
            await _unitOfWork.CartRepository.UpdateItemAsync(condition);
            await _unitOfWork.SaveChangeAsync();
        }
        public async Task RemoveCartItemAsync(CartItemInfo cartItem)
        {
            var cartId = await _unitOfWork.CartRepository.GetIdAsync(cartItem.MemberId);
            var condition = new CartItemCondition
            {
                CartId = cartId,
                ProductId = cartItem.ProductId
            };
            await _unitOfWork.CartRepository.RemoveItemAsync(condition);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<CartItemDataModel>> CallStripeCheckOut(int memberId)
        {
            var cartItems = await _unitOfWork.CartRepository.GetItemsEnumAsync(memberId);
            return cartItems;
        }

    }
}
