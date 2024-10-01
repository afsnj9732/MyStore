namespace MyStore.Server.Controllers.Dtos.ViewModels
{
    public class ProductsViewModel
    {
        public int TotalPage { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
