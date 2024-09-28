namespace MyStore.Server.Controllers.Dtos.ViewModels
{
    public class CartViewModel
    {
        public int CartId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
