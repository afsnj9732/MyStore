using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Repository.Interfaces;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace MyStore.Server.Models.Repository.Implements
{
    public class CartRepository : ICartRepository
    {
        private readonly DbStoreContext _db;

        public CartRepository(DbStoreContext db)
        {
            _db = db;
        }
        public async Task<int> GetCartIdAsync(int memberId)
        {
            var cart = await _db.TCarts.Where(cart => cart.MemberId == memberId).FirstOrDefaultAsync();
            var result = cart.CartId;
            return result;
        }
        public async Task AddCartItemAsync(CartItemCondition cartItem)
        {
            var target = await _db.TCartItems.Where(item => item.ProductId == cartItem.ProductId
            && item.CartId == cartItem.CartId).FirstOrDefaultAsync();
            if (target == null)
            {
                var AddItem = new TCartItem
                {
                    CartId = cartItem.CartId,
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity
                };
                await _db.TCartItems.AddAsync(AddItem);
            }
            else
            {
                target.Quantity += cartItem.Quantity;
            }

        }

        public async Task CreateCartAsync(int memberId)
        {
            var cart = new TCart { MemberId = memberId };
            await _db.TCarts.AddAsync(cart);
        }

        public async Task<IEnumerable<CartItemDataModel>> GetCartItemsEnumByUserIdAsync(int memberId)
        {
            var cart = await _db.TCarts.Where(cart=>cart.MemberId == memberId).FirstOrDefaultAsync();
            var cartItemsEnum = await _db.TCartItems.Include(cartItem=>cartItem.Product).Where(cartItem => cartItem.CartId == cart.CartId).ToListAsync();

            var result = cartItemsEnum.Select(item => new CartItemDataModel
            {
                CartId = item.CartId,
                ProductId = item.ProductId,
                ProductName = item.Product?.Name ?? "",
                Quantity = item.Quantity,
                Price = item.Product?.Price ?? 0 
            });
            return result;
        }

        public async Task UpdateCartItemAsync(CartItemCondition cartItem)
        {
            var getUspUpdateItem = await _db.TCartItems.FromSqlRaw("EXEC usp_FindUpdateCartItem @CartId,@ProductId",
                new SqlParameter("@CartId", cartItem.CartId),
                new SqlParameter("@ProductId", cartItem.ProductId)).ToListAsync();
            var updateItem = getUspUpdateItem.FirstOrDefault();
            updateItem.Quantity = cartItem.Quantity;
        }
        public async Task RemoveCartItemAsync(CartItemCondition cartItem)
        {
            var RemoveItem = await _db.TCartItems.Where(item=>item.CartId==cartItem.CartId
            && item.ProductId==cartItem.ProductId).FirstOrDefaultAsync();
            _db.TCartItems.Remove(RemoveItem);
        }

        public async Task RemoveUserAllCartItemsAsync(int memberId)
        {
            var RemoveItems = await _db.TCartItems.Where(item => item.Cart.MemberId == memberId).ToListAsync();
            _db.TCartItems.RemoveRange(RemoveItems);
        }

    }
}
