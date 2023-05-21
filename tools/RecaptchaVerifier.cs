namespace IB_projekat.tools
{
    using Newtonsoft.Json.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class RecaptchaVerifier
    {
        private readonly HttpClient _httpClient;
        private const string VerificationUrl = "https://www.google.com/recaptcha/api/siteverify";
        private readonly string _secretKey;

        public RecaptchaVerifier(string secretKey)
        {
            _httpClient = new HttpClient();
            _secretKey = secretKey;
        }

        public async Task<bool> VerifyRecaptcha(string responseToken)
        {
            var parameters = new Dictionary<string, string>
        {
            { "secret", _secretKey },
            { "response", responseToken }
        };

            var response = await _httpClient.PostAsync(VerificationUrl, new FormUrlEncodedContent(parameters));
            var responseBody = await response.Content.ReadAsStringAsync();

            var verificationResult = JObject.Parse(responseBody);
            var success = verificationResult.Value<bool>("success");
            var score = verificationResult.Value<double>("score");

            var scoreThreshold = 0.5;

            return success && score >= scoreThreshold;
        }
    }
}
