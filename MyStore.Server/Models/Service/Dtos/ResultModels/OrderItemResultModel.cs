namespace MyStore.Server.Models.Service.Dtos.ResultModels
{
    public class OrderItemResultModel
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
