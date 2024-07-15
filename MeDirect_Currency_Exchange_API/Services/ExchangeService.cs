using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Exceptions;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Models;
using MeDirect_Currency_Exchange_API.Models.DTOs;
using Serilog;

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
            Log.Information("Received request to create trade: {@TradeRequest}", tradeRequest);

            var tradesInLastHour = await _tradeRepository.GetTradesForClientBetweenDatesAsync(
                tradeRequest.ID_Client,
                DateTime.UtcNow.AddHours(-1),
                null
            );

            if(tradesInLastHour.Count() >= 10) {
                Log.Warning("Client {ClientId} has reached the maximum limit of 10 trades per hour.", tradeRequest.ID_Client);
                throw new ApiException(429, "Reached the maximum limit of 10 trades per hour.");
            }
            var rate = await GetRateAsync(tradeRequest.FromCurrency, tradeRequest.ToCurrency);
            if(rate == null) {
                Log.Warning("Exchange rate not found for {FromCurrency} to {ToCurrency}", tradeRequest.FromCurrency, tradeRequest.ToCurrency);
                throw new ApiException(404, "Exchange rate not found");
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
            Log.Information("Trade created successfully: {@Trade}", trade);
            return trade;
        }

        public async Task<decimal?> GetRateAsync(string fromCurrency, string toCurrency) {
            Log.Information("Getting exchange rate from {FromCurrency} to {ToCurrency}", fromCurrency, toCurrency);
            // Check cache first
            var cacheKey = $"{fromCurrency}";
            var cachedRates = _cacheService.Get<CurrencyRate>(cacheKey);
            if(cachedRates != null) {
                if(cachedRates.Rates.TryGetValue(toCurrency, out var cachedRate)) {
                    Log.Information("Exchange rate found in Cache {Rate}", cachedRate);
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
                if(rateProvider.Rates.TryGetValue(toCurrency, out var providerRate)) {
                    Log.Information("Exchange rate from provider: {Rate}", providerRate);
                    return providerRate;
                }
            }
            Log.Warning("Rate from {FromCurrency} to {ToCurrency} not found", fromCurrency, toCurrency);
            return null;
        }
    }
}
