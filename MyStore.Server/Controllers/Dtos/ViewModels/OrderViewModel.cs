
namespace MyStore.Server.Controllers.Dtos.ViewModels
{
    public class OrderViewModel
    {
        public int TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public virtual ICollection<OrderItemViewModel> TOrderItems { get; set; } = new List<OrderItemViewModel>();
    }
}
