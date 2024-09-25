
namespace MyStore.Server.Models.Service.Dtos.ResultModels
{
    public class CartResultModel
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
