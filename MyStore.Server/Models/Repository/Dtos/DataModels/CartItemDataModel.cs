
namespace MyStore.Server.Models.Repository.Dtos.DataModels
{
    public class CartItemDataModel
    {

        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
