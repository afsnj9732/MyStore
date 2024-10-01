namespace MyStore.Server.Controllers.Dtos.ViewModels
{
    public class CartViewModel
    {
        public int TotalPrice { get; set; }
        public IEnumerable<CartItemViewModel> CartItems { get; set; }
    }
}
