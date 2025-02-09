using MyStore.Server.Models.Service.Dtos.Infos;

namespace MyStore.Server.Models.Service.Interfaces
{
    public interface IStripeService
    {
        Task<bool> CheckPayment(string session_id);
        Task<string> CreateStripeAsync(StripeInfo stripeInfo);
    }
}
