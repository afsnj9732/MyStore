
namespace MyStore.Server.Models.Repository.Dtos.DataModels
{
    public class OrderDataModel
    {
        public int TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual ICollection<OrderItemDataModel> TOrderItems { get; set; } = new List<OrderItemDataModel>();
    }
}
