namespace MyStore.Server.Controllers.Dtos.ViewModels
{
    public class CartViewModel
    {
        public int TotalPrice { get; set; }
        public List<CartItemViewModel> CartItems { get; set; }
    }
}
