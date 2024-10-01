namespace MyStore.Server.Models.Service.Dtos.ResultModels
{
    public class OrderResultModel
    {
        public int TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public  IEnumerable<OrderItemResultModel> TOrderItems { get; set; }
    }
}
