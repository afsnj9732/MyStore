using MyStore.Server.Models.Factories.Interfaces;
using MyStore.Server.Models.Service.Implements;
using MyStore.Server.Models.Service.Interfaces;

namespace MyStore.Server.Models.Factories.Implements
{
    public class PaymentFactory : IPaymentFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public PaymentFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IPaymentService CreatePaymentService(string paymentMethod)
        {
            switch (paymentMethod)
            {
                case "stripe":
                    return _serviceProvider.GetRequiredService<StripeService>();
                case "stripeEmbedded":
                    return _serviceProvider.GetRequiredService<StripeEmbeddedService>();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
