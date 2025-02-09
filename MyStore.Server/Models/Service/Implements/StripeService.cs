using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace MyStore.Server.Models.Service.Implements
{
    public class StripeService:IStripeService
    {
        private readonly IConfiguration _configuration;
        public StripeService(IConfiguration configuration) 
        {
            _configuration = configuration;
        }

        public async Task<string> CreateStripeAsync(StripeInfo stripeInfo)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            var OrderItems = new List<SessionLineItemOptions>();
            foreach(var item in stripeInfo.CartItems)
            {
                var orderItem = new SessionLineItemOptions
                {
                    Price = item.StripePriceID.TrimEnd(),
                    Quantity = item.Quantity,
                };
                OrderItems.Add(orderItem);
            }
            
            var options = new SessionCreateOptions
            {
                LineItems = OrderItems,
                Mode = "payment",
                ClientReferenceId=Guid.NewGuid().ToString(),
                SuccessUrl = _configuration["Domain:local"] + "/products",
                CancelUrl = _configuration["Domain:local"] + "/products",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return session.Url;
            
        }
    }
}
