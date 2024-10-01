using MyStore.Server.Models.Repository.Dtos.Conditions;
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

        public async Task<int> GetTotalPriceAsync(int memberId)
        {
            var cartItemsList = await _unitOfWork.CartRepository.GetCartItemsEnumByUserIdAsync(memberId);
            var result = cartItemsList
                .Sum(item=>item.Price*item.Quantity);
            return result;
        }
        public async Task<IEnumerable<CartResultModel>?> GetCartItemsAsync(int memberId)
        {
            var cartItemsList = await _unitOfWork.CartRepository.GetCartItemsEnumByUserIdAsync(memberId);
            var result = cartItemsList.Select(item => new CartResultModel
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductStockQuantity = item.ProductStockQuantity,
                Quantity = item.Quantity,
                Price = item.Price
            });
            return result;
        }

        public async Task<int> GetCartItemCountAsync(int memberId)
        {
            var CartItems = await _unitOfWork.CartRepository.GetCartItemsEnumByUserIdAsync(memberId);
            var result = CartItems?.Sum(item => item.Quantity) ?? 0;
            return result;
        }

        public async Task AddCartItemAsync(CartItemInfo cartItem)
        {
            var cartId = await _unitOfWork.CartRepository.GetCartIdAsync(cartItem.MemberId);
            var condition = new CartItemCondition
            {
                CartId = cartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };
            await _unitOfWork.CartRepository.AddCartItemAsync(condition);
            await _unitOfWork.Save();
        }
        public async Task UpdateCartItemAsync(CartItemInfo cartItem)
        {
            var cartId = await _unitOfWork.CartRepository.GetCartIdAsync(cartItem.MemberId);
            var condition = new CartItemCondition
            {
                CartId = cartId,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity
            };
            await _unitOfWork.CartRepository.UpdateCartItemAsync(condition);
            await _unitOfWork.Save();
        }
        public async Task RemoveCartItemAsync(CartItemInfo cartItem)
        {
            var cartId = await _unitOfWork.CartRepository.GetCartIdAsync(cartItem.MemberId);
            var condition = new CartItemCondition
            {
                CartId = cartId,
                ProductId = cartItem.ProductId
            };
            await _unitOfWork.CartRepository.RemoveCartItemAsync(condition);
            await _unitOfWork.Save();
        }
    }
}
