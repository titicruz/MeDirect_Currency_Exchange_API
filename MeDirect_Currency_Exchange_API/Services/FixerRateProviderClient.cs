using MeDirect_Currency_Exchange_API.Exceptions;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Models;
using Newtonsoft.Json;
using Serilog;

namespace MeDirect_Currency_Exchange_API.Services {
    public class FixerRateProviderClient : IRateProviderClient {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        public FixerRateProviderClient(HttpClient httpClient, IConfiguration configuration) {
            _httpClient = httpClient;
            _apiKey = configuration["FixerApi:ApiKey"];
            _baseUrl = configuration["FixerApi:BaseUrl"];
        }
        public async Task<CurrencyRate> GetRateAsync(string currency) {
            Log.Information($"Getting Rates from Provider for {currency}");
            var url = $"{_baseUrl}/latest?access_key={_apiKey}&base={currency}";
            Log.Debug("Constructed URL: {Url}", url);
            HttpResponseMessage response;
            try {
                response = await _httpClient.GetAsync(url);
            }
            catch(Exception ex) {
                Log.Error(ex, "Error occurred while sending request to Fixer API for currency {Currency}", currency);
                throw new ApiException(1000, "Error on requesting");
            }
            if(!response.IsSuccessStatusCode) {
                Log.Warning("Received unsuccessful status code {StatusCode} from Fixer API for currency {Currency}", response.StatusCode, currency);
                throw new ApiException(1000, "Error on requesting");
            }
            var content = await response.Content.ReadAsStringAsync();
            Log.Debug("Received response content: {Content}", content);
            FixerResponse result = JsonConvert.DeserializeObject<FixerResponse>(content);

            if(result == null)
                throw new ApiException(1001, "Error Obtaining result");
            if(!result.Success)
                throw new ApiException(result.Error.Code, result.Error.Info);

            var rate = new CurrencyRate {
                BaseCurrency = currency,
                Rates = result.Rates,
                FetchedAt = DateTime.UtcNow
            };
            Log.Information("Rates for currency {Currency}: {Rates}", currency, rate.Rates);
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
            public string Info { get; set; }
        }
    }
}

