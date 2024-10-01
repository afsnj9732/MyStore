namespace MyStore.Server.Controllers.Dtos.ViewModels
{
    public class ProductsViewModel
    {
        public int TotalPage { get; set; }
        public List<ProductViewModel> Products { get; set; }
    }
}
