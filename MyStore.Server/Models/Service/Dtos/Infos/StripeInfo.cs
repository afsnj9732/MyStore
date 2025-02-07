using MyStore.Server.Models.Repository.Dtos.DataModels;

namespace MyStore.Server.Models.Service.Dtos.Infos
{
    public class StripeInfo
    {
        public IEnumerable<CartItemDataModel> CartItems { get; set; }
    }
}
