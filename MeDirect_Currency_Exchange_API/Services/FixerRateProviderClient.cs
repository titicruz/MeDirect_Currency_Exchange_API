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
            Log.Information($"Getting Rates from {currency}");
            var url = $"{_baseUrl}/latest?access_key={_apiKey}&base={currency}";
            var response = await _httpClient.GetAsync(url);

            if(!response.IsSuccessStatusCode) {
                throw new ApiException(1000, "Error on requesting");
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<FixerResponse>(content);

            if(result == null || !result.Success) {
                if(result == null)
                    throw new ApiException(1001, "Error Obtaining result");
                if(!result.Success)
                    throw new ApiException(result.Error.Code, result.Error.Info);
                throw new ApiException(1002, "No currency obtained");
            }

            var rate = new CurrencyRate {
                BaseCurrency = currency,
                Rates = result.Rates,
                FetchedAt = DateTime.UtcNow
            };
            Log.Information($"Response {rate}");
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

