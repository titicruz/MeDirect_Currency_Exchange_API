using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Data.Repositories;
using MeDirect_Currency_Exchange_API.Exceptions;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Models;
using MeDirect_Currency_Exchange_API.Models.DTOs;

namespace MeDirect_Currency_Exchange_API.Services {
    public class ExchangeService : IExchangeService {
        private IRateProviderClient _rateClient;
        private ITradeRepository _tradeRepository;
        private readonly ICacheService _cacheService;
        public ExchangeService(IRateProviderClient rateProviderClient, ICacheService cacheService, ITradeRepository tradeRepository) {
            _rateClient = rateProviderClient;
            _cacheService = cacheService;
            _tradeRepository = tradeRepository;
        }

        public async Task<Trade> CreateTradeAsync(TradeRequest tradeRequest) {
            var tradesInLastHour = await _tradeRepository.GetTradesForClientBetweenDatesAsync(
                tradeRequest.ID_Client,
                DateTime.UtcNow.AddHours(-1),
                null
            );

            if (tradesInLastHour.Count() >= 10) {
                throw new ApiException(429,"Client has reached the maximum limit of 10 trades per hour.");
            }
            var rate = await GetRateAsync(tradeRequest.FromCurrency, tradeRequest.ToCurrency);
            if (rate == null) {
                throw new ApiException(404,"Exchange rate not found");
            }
            var trade = new Trade {
                ID_Client = tradeRequest.ID_Client,
                FromCurrency = tradeRequest.FromCurrency,
                ToCurrency = tradeRequest.ToCurrency,
                Amount = tradeRequest.Amount,
                Rate = rate.Value,
                Dt_Create = DateTime.UtcNow
            };

            // Calculate exchanged amount
            trade.ExchangedAmount = trade.Amount * trade.Rate; 
            await _tradeRepository.AddTradeAsync(trade);

            return trade;
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
