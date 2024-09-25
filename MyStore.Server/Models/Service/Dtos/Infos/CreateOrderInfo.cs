namespace MyStore.Server.Models.Service.Dtos.Infos
{
    public class CreateOrderInfo
    {
        public int MemberId { get; set; }
        public string StripeToken { get; set; }
    }
}
