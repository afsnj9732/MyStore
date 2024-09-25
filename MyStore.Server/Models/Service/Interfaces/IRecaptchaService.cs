namespace MyStore.Server.Models.Service.Interfaces
{
    public interface IRecaptchaService
    {
        Task<bool> VerifyRecaptchaAsync(string responseToken);
    }
}
