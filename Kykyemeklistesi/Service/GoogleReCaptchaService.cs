using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

public class GoogleReCaptchaService
{
    private readonly IConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;

    public GoogleReCaptchaService(IConfiguration config, IHttpClientFactory httpClientFactory)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> VerifyToken(string token)
    {
        
        var secret = _config["GoogleReCaptcha:SecretKey"];

        var client = _httpClientFactory.CreateClient();

        var parameters = new Dictionary<string, string>
        {
            { "secret", secret },
            { "response", token }
        };

        var content = new FormUrlEncodedContent(parameters);

        var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);

        var jsonString = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(jsonString);

        return jsonDoc.RootElement.GetProperty("success").GetBoolean();
    }
}
