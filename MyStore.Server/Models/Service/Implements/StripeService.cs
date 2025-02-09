using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace MyStore.Server.Models.Service.Implements
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;
        public StripeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<bool> CheckPayment(string sessionId)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            var options = new SessionGetOptions
            {
                Expand = new List<string> { "line_items" },
            };

            var service = new SessionService();
            var checkoutSession = service.Get(sessionId, options);
            if (checkoutSession.PaymentStatus == "paid")
            {
                return true;
            }
            return false;
        }

        public async Task<string> CreateStripeAsync(StripeInfo stripeInfo)
        {
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
            var OrderItems = new List<SessionLineItemOptions>();
            foreach (var item in stripeInfo.CartItems)
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
                ClientReferenceId = Guid.NewGuid().ToString(),
                SuccessUrl = _configuration["Domain:local"] + "/stripe"
                + "?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = _configuration["Domain:local"] + "/cart",
            };

            var service = new SessionService();
            Session session = await service.CreateAsync(options);

            return session.Url;

        }
    }
}
