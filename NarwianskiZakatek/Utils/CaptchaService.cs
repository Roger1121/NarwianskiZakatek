using Newtonsoft.Json.Linq;

namespace NarwianskiZakatek.Services
{
    public class CaptchaService : ICaptchaService
    {
        private readonly HttpClient _client;
        private readonly CaptchaConfig _config;

        public CaptchaService(HttpClient captchaClient, CaptchaConfig config)
        {
            _client = captchaClient;
            _config = config;
        }

        public async Task<bool> IsValid(string captcha)
        {
            try
            {
                var postTask = await _client
                    .PostAsync($"https://www.google.com/recaptcha/api/siteverify?secret={_config.PrivateKey}&response={captcha}", new StringContent(""));
                var result = await postTask.Content.ReadAsStringAsync();
                var resultObject = JObject.Parse(result);
                dynamic success = resultObject["success"];
                return (bool)success;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }
        }
    }
}
