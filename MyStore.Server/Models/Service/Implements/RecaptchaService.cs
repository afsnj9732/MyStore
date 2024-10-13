using MyStore.Server.Models.Service.Interfaces;
using Newtonsoft.Json.Linq;

namespace MyStore.Server.Models.Service.Implements
{
    public class RecaptchaService:IRecaptchaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<RecaptchaService> _logger;
        public RecaptchaService(HttpClient httpClient,IConfiguration configuration, ILogger<RecaptchaService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
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
            catch(Exception ex)
            {
                _logger.LogError("Recaptcha請求失敗,參考訊息:{Message}",ex.Message);
                return false;
            }
        }
    }
}
