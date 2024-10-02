namespace MyStore.Server.Models.Repository.Dtos.Conditions
{
    public class CartItemCondition
    {
        public int? CartId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
