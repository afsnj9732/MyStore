
namespace MyStore.Server.Models.Service.Dtos.ResultModels
{
    public class OrderResultModel
    {
        public int TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual ICollection<OrderItemResultModel> TOrderItems { get; set; } = new List<OrderItemResultModel>();
    }
}
