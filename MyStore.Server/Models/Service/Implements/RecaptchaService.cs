using MyStore.Server.Models.Service.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace MyStore.Server.Models.Service.Implements
{
    public class RecaptchaService:IRecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public RecaptchaService(HttpClient httpClient,IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> VerifyRecaptchaAsync(string responseToken)
        {
            try
            {
                var response = await _httpClient.PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_configuration["Recaptcha:Secret"]}&response={responseToken}", null);
                response.EnsureSuccessStatusCode();
                var responseResult = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(responseResult);
                return (bool)result["success"];
            }
            catch
            {
                return false;
            }
        }
    }
}
