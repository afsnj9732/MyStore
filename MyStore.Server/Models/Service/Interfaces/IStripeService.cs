using MyStore.Server.Models.Service.Dtos.Infos;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface IStripeService
    {
        void CreateOrder(StripeInfo stripeInfo);
    }
}
