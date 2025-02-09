namespace MyStore.Server.Models.Service.Dtos.ResultModels
{
    public class OrderItemResultModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
