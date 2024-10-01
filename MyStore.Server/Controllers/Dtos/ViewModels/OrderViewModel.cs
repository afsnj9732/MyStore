
namespace MyStore.Server.Controllers.Dtos.ViewModels
{
    public class OrderViewModel
    {
        public int TotalPrice { get; set; }

        public DateTime OrderDate { get; set; }

        public IEnumerable<OrderItemViewModel> TOrderItems { get; set; }
    }
}
