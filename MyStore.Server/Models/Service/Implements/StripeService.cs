using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Interfaces;
using Stripe;

namespace MyStore.Server.Models.Service.Implements
{
    public class StripeService:IStripeService
    {
        private readonly IConfiguration _configuration;
        public StripeService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public void CreateOrder(StripeInfo stripeInfo)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            var options = new ChargeCreateOptions
            {
                Amount = stripeInfo.TotalPrice * 100,
                Currency = "twd",
                Description = "付款",
                Source = stripeInfo.StripeToken,
            };

            var service = new ChargeService();
            service.Create(options);
        }
    }
}
