using MeDirect_Currency_Exchange_API.Exceptions;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Models;
using Newtonsoft.Json;

namespace MeDirect_Currency_Exchange_API.Services {
    public class FixerRateProviderClient : IRateProviderClient {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly ILogger<FixerRateProviderClient> _logger;
        public FixerRateProviderClient(HttpClient httpClient, IConfiguration configuration, ILogger<FixerRateProviderClient> logger) {
            _httpClient = httpClient;
            _apiKey = configuration["FixerApi:ApiKey"];
            _baseUrl = configuration["FixerApi:BaseUrl"];
            _logger = logger;
        }
        public async Task<CurrencyRate> GetRateAsync(string currency) {
            _logger.LogInformation($"Getting Rates from Provider for {currency}");
            var url = $"{_baseUrl}/latest?access_key={_apiKey}&base={currency}";
            _logger.LogDebug("Constructed URL: {Url}", url);
            HttpResponseMessage response;
            try {
                response = await _httpClient.GetAsync(url);
            }
            catch(Exception ex) {
                _logger.LogError(ex, "Error occurred while sending request to the Provider for the currency {Currency}", currency);
                throw new ApiException(1000, "Error on requesting", "Internal Error");
            }
            if(!response.IsSuccessStatusCode) {
                _logger.LogWarning("Received unsuccessful status code {StatusCode} from the Provider API for currency {Currency}", response.StatusCode, currency);
                throw new ApiException(1000, "Error on requesting", "API Error");
            }
            var content = await response.Content.ReadAsStringAsync();
            _logger.LogDebug("Received response content: {Content}", content);
            FixerResponse result = JsonConvert.DeserializeObject<FixerResponse>(content);

            if(result == null)
                throw new ApiException(1001, "Error Obtaining result", "API Error");
            if(!result.Success)
                throw new ApiException(result.Error.Code, result.Error.type, "API Error");

            var rate = new CurrencyRate {
                BaseCurrency = currency,
                Rates = result.Rates,
                FetchedAt = DateTime.UtcNow
            };
            _logger.LogInformation("Rates for currency {Currency}: {Rates}", currency, rate.Rates);
            return rate;
        }
        private class FixerResponse {
            public bool Success { get; set; }
            public int Timestamp { get; set; }
            public string Base { get; set; }
            public string Date { get; set; }
            public Dictionary<string, decimal> Rates { get; set; }
            public ErrorDetails Error { get; set; }
        }
        private class ErrorDetails {
            public int Code { get; set; }
            public string type { get; set; }
        }
    }
}

