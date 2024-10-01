namespace MyStore.Server.Models.Repository.Dtos.Conditions
{
    public class OrderCondition
    {
        public int MemberId { get; set; }
        public int TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
