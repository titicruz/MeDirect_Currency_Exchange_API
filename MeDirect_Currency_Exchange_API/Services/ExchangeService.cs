using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Exceptions;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Models;
using MeDirect_Currency_Exchange_API.Models.DTOs;

namespace MeDirect_Currency_Exchange_API.Services {
    public class ExchangeService : IExchangeService {
        private IRateProviderClient _rateClient;
        private ITradeRepository _tradeRepository;
        private readonly ICacheService _cacheService;
        private IClientRepository _clientRepository;
        private readonly ILogger<ExchangeService> _logger;
        private readonly int _hourLimit = 1;
        private readonly int _tradeLimit = 10;
        private readonly int _cacheTime = 30;
        public ExchangeService(IRateProviderClient rateProviderClient, ICacheService cacheService, ITradeRepository tradeRepository, IClientRepository clientRepository, ILogger<ExchangeService> logger, IConfiguration configuration) {
            _rateClient = rateProviderClient;
            _cacheService = cacheService;
            _tradeRepository = tradeRepository;
            _clientRepository = clientRepository;
            _logger = logger;
            _cacheTime = configuration.GetValue<int>("Configurations:CacheTime");
            _tradeLimit = configuration.GetValue<int>("Configurations:TradeLimit");
            _hourLimit = configuration.GetValue<int>("Configurations:HourLimit");
        }

        public async Task<Trade> CreateTradeAsync(TradeRequest tradeRequest) {
            _logger.LogInformation("Received request to create trade: {@TradeRequest}", tradeRequest);
            var client = await _clientRepository.GetClientByIdAsync(tradeRequest.ID_Client);
            if(client == null) {
                _logger.LogWarning("Client {ClientId} doesn't exists.", tradeRequest.ID_Client);
                throw new ApiException(404, "The Client doesn't exists", "Not found");
            }
            var tradesInLastHour = await _tradeRepository.GetCountTradesForClientBetweenDatesAsync(
                tradeRequest.ID_Client,
                DateTime.UtcNow.AddHours(_hourLimit * -1),
                null
            );

            if(tradesInLastHour >= _tradeLimit) {
                _logger.LogWarning("Client {ClientId} has reached the maximum limit of 10 trades per hour.", tradeRequest.ID_Client);
                throw new ApiException(429, "Reached the maximum limit of 10 trades per hour.", "Limit Error");
            }
            var rate = await GetRateAsync(tradeRequest.FromCurrency, tradeRequest.ToCurrency);
            if(rate == null) {
                _logger.LogWarning("Exchange rate not found for {FromCurrency} to {ToCurrency}", tradeRequest.FromCurrency, tradeRequest.ToCurrency);
                throw new ApiException(404, "Exchange rate not found", "Not Found");
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
            _logger.LogInformation("Trade created successfully: {@Trade}", trade);
            return trade;
        }

        public async Task<decimal?> GetRateAsync(string fromCurrency, string toCurrency) {
            _logger.LogInformation("Getting exchange rate from {FromCurrency} to {ToCurrency}", fromCurrency, toCurrency);
            // Check cache first
            var cacheKey = $"{fromCurrency}";
            var cachedRates = _cacheService.Get<CurrencyRate>(cacheKey);
            if(cachedRates != null) {
                if(cachedRates.Rates.TryGetValue(toCurrency, out var cachedRate)) {
                    _logger.LogInformation("Exchange rate found in Cache {Rate}", cachedRate);
                    return cachedRate;
                }
            }
            // Get rate from provider
            var rateProvider = await _rateClient.GetRateAsync(fromCurrency);

            _cacheService.Set(cacheKey, rateProvider, TimeSpan.FromMinutes(_cacheTime));
            if(rateProvider.Rates != null) {
                if(rateProvider.Rates.TryGetValue(toCurrency, out var providerRate)) {
                    _logger.LogInformation("Exchange rate from provider: {Rate}", providerRate);
                    return providerRate;
                }
            }
            _logger.LogWarning("Rate from {FromCurrency} to {ToCurrency} not found", fromCurrency, toCurrency);
            return null;
        }
    }
}
