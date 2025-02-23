using MyStore.Server.Models.Service.Interfaces;

namespace MyStore.Server.Models.Factories.Interfaces
{
    public interface IPaymentFactory
    {
        IPaymentService CreatePaymentService(string paymentMethod);
    }
}
