using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Interfaces;

namespace MyStore.Server.Models.Service.Implements
{
    public class StripeEmbeddedService : IPaymentService
    {
        public bool CheckPayment(string session_id)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateStripeAsync(StripeInfo stripeInfo)
        {
            throw new NotImplementedException();
        }
    }
}
