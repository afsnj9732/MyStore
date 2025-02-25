using MyStore.Server.Models.Factories.Interfaces;
using MyStore.Server.Models.Service.Implements;
using MyStore.Server.Models.Service.Interfaces;

namespace MyStore.Server.Models.Factories.Implements
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly StripeService _stripeService;
        private readonly StripeEmbeddedService _stripeEmbeddedService;
        public PaymentFactory(StripeService stripeService, StripeEmbeddedService stripeEmbeddedService)
        {
            _stripeService = stripeService;
            _stripeEmbeddedService = stripeEmbeddedService;
        }

        public IPaymentService CreatePaymentService(string paymentMethod)
        {
            switch (paymentMethod)
            {
                case "stripe":
                    return _stripeService;
                case "stripeEmbedded":
                    return _stripeEmbeddedService;
                default:
                    throw new NotImplementedException(paymentMethod+"錯誤");
            }
        }
    }
}
