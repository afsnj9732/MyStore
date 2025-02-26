using MyStore.Server.Models.Factories.Interfaces;
using MyStore.Server.Models.Service.Implements;
using MyStore.Server.Models.Service.Interfaces;

namespace MyStore.Server.Models.Factories.Implements
{
    public class PaymentFactory : IPaymentFactory
    {
        //private readonly IServiceProvider _serviceProvider;
        private readonly StripeService _stripeService;
        private readonly StripeEmbeddedService _stripeEmbeddedService;
        public PaymentFactory(StripeService stripeService, StripeEmbeddedService stripeEmbeddedService/*IServiceProvider serviceProvider*/)
        {
            _stripeService = stripeService;
            _stripeEmbeddedService = stripeEmbeddedService;
            //_serviceProvider = serviceProvider;
        }

        public IPaymentService CreatePaymentService(string paymentMethod)
        {
            switch (paymentMethod)
            {
                case "stripe":
                    return _stripeService;
                    //return _serviceProvider.GetRequiredService<StripeService>();
                case "stripeEmbedded":
                    return _stripeEmbeddedService;
                    //return _serviceProvider.GetRequiredService<StripeEmbeddedService>();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
