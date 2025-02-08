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

        public string CreateOrder(StripeInfo stripeInfo)
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
                UiMode = "embedded",
                LineItems = OrderItems,
                Mode = "payment",
                //ClientReferenceId=Guid.NewGuid().ToString(),
                ReturnUrl = _configuration["Domain:local"] + "/orders",
                //CancelUrl = _configuration["Domain:local"] + "/cart",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session.ClientSecret;
            
        }
    }
}
