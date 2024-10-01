namespace MyStore.Server.Models.Service.Dtos.ResultModels
{
    public class ProductsResultModel
    {
        public int TotalPage { get; set; }
        public IEnumerable<ProductResultModel> Products { get; set; }
    }
}
