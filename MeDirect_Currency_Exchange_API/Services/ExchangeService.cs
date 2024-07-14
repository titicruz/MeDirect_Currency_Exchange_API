using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Models;
using MeDirect_Currency_Exchange_API.Models.DTOs;

namespace MeDirect_Currency_Exchange_API.Services {
    public class ExchangeService : IExchangeService {
        private IRateProviderClient _rateClient;
        private readonly ICacheService _cacheService;
        public ExchangeService(IRateProviderClient rateProviderClient, ICacheService cacheService) {
            _rateClient = rateProviderClient;
            _cacheService = cacheService;
        }

        public Task<Trade> CreateTradeAsync(TradeRequest tradeRequest) {
            throw new NotImplementedException();
        }

        public async Task<decimal?> GetRateAsync(string fromCurrency, string toCurrency) {
            // Check cache first
            var cacheKey = $"{fromCurrency}";
            var cachedRates = _cacheService.Get<CurrencyRate>(cacheKey);
            if(cachedRates != null) {
                if(cachedRates.Rates.TryGetValue(toCurrency, out var cachedRate)) {
                    return cachedRate;
                }
            }

            // Get rate from provider
            var rateProvider = await _rateClient.GetRateAsync(fromCurrency);
            var currencyRate = new CurrencyRate {
                BaseCurrency = fromCurrency,
                Rates = rateProvider.Rates,
                FetchedAt = DateTime.UtcNow
            };
            _cacheService.Set(cacheKey, rateProvider, TimeSpan.FromMinutes(30));
            if(rateProvider.Rates != null) {
                if(rateProvider.Rates.TryGetValue(toCurrency, out var cachedRate)) {
                    return cachedRate;
                }
            }
            return null;
        }
    }
}
