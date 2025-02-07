using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Dtos.Conditions;
using MyStore.Server.Models.Repository.Dtos.DataModels;
using MyStore.Server.Models.Repository.Interfaces;

namespace MyStore.Server.Models.Repository.Implements
{
    public class CartRepository : ICartRepository
    {
        private readonly DbStoreContext _db;

        public CartRepository(DbStoreContext db)
        {
            _db = db;
        }
        public async Task<int?> GetIdAsync(int memberId)
        {
            var cart = await _db.TCarts.Where(cart => cart.MemberId == memberId).FirstOrDefaultAsync();
            if (cart == null) {
                return null;
            }
            var result = cart.CartId;
            return result;
        }
        public async Task AddItemAsync(CartItemCondition cartItem)
        {
            var target = await _db.TCartItems.Where(item => item.ProductId == cartItem.ProductId
            && item.CartId == cartItem.CartId).FirstOrDefaultAsync();
            if (target == null)
            {
                    var AddItem = new TCartItem
                    {
                        CartId = cartItem.CartId ?? throw new ArgumentException("CartId 不能為 null"),
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

        public async Task CreateAsync(int memberId)
        {
            var cart = new TCart { MemberId = memberId };
            await _db.TCarts.AddAsync(cart);
        }

        public async Task<IEnumerable<CartItemDataModel>?> GetItemsEnumAsync(int memberId)
        {
            //var cart = await _db.TCarts.Where(cart=>cart.MemberId == memberId).FirstOrDefaultAsync();
            //if(cart == null)
            //{
            //    return null;
            //}
            //var cartItemsEnum = await _db.TCartItems
            //    .Where(cartItem => cartItem.CartId == cart.CartId)
            //    .Include(cartItem => cartItem.Product)
            //    .ToListAsync();

            //var result = cartItemsEnum.Select(item => new CartItemDataModel
            //{
            //    ProductId = item.ProductId,
            //    ProductName = item.Product?.Name ?? "",
            //    ProductStockQuantity = item.Product?.StockQuantity ?? 0,
            //    Quantity = item.Quantity,
            //    Price = item.Product?.Price ?? 0 
            //});


            var result = await _db.CartItemDTO.FromSqlRaw("EXEC usp_GetCartItems @MemberId"
, new SqlParameter("@MemberId", memberId)).ToListAsync();
            return result;
        }

        public async Task UpdateItemAsync(CartItemCondition cartItem)
        {
            var getUspUpdateItem = await _db.TCartItems.FromSqlRaw("EXEC usp_FindUpdateCartItem @CartId,@ProductId",
                new SqlParameter("@CartId", cartItem.CartId),
                new SqlParameter("@ProductId", cartItem.ProductId)).ToListAsync();
            var updateItem = getUspUpdateItem.FirstOrDefault();
            if (updateItem != null)
            {
                updateItem.Quantity = cartItem.Quantity;
            }
        }
        public async Task RemoveItemAsync(CartItemCondition cartItem)
        {
            var RemoveItem = await _db.TCartItems.Where(item=>item.CartId==cartItem.CartId
            && item.ProductId==cartItem.ProductId).FirstOrDefaultAsync();
            if (RemoveItem != null) {
                _db.TCartItems.Remove(RemoveItem);
            }
        }

        public async Task RemoveAllItemsAsync(int memberId)
        {
            var RemoveItems = await _db.TCartItems.Include(item=>item.Cart).Where(item => item.Cart.MemberId == memberId).ToListAsync();
            _db.TCartItems.RemoveRange(RemoveItems);
        }

    }
}
