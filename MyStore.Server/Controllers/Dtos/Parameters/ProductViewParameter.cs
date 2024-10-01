namespace MyStore.Server.Controllers.Dtos.Parameters
{
    public class ProductViewParameter
    {
        public int Page { get; set; } = 1;
        public string SearchWord { get; set; } = "";
    }
}
