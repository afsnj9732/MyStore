using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace MyStore.Server.Models.Service.Implements
{
    public class StripeEmbeddedService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        public StripeEmbeddedService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool CheckPayment(string sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreatePaymentAsync(PaymentInfo stripeInfo)
        {
            throw new NotImplementedException();
        }
    }
}
