namespace MyStore.Server.Models.Service.Dtos.ResultModels
{
    public class CartResultModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductStockQuantity { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
        public string StripePriceID { get; set; }
    }
}
