namespace MyStore.Server.Controllers.Dtos.ViewModels
{
    public class CartViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductStockQuantity { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
