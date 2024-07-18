using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Exceptions;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Models;
using MeDirect_Currency_Exchange_API.Models.DTOs;
using MeDirect_Currency_Exchange_API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace MeDirect_Currency_Exchange_API.test {
    public class ExchangeServiceTests {
        private readonly Mock<IRateProviderClient> _mockRateProviderClient;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly Mock<ITradeRepository> _mockTradeRepository;
        private readonly Mock<IClientRepository> _mockClientRepository;
        private readonly Mock<ILogger<ExchangeService>> _mockLogger;
        private readonly ExchangeService _exchangeService;

        public ExchangeServiceTests() {
            _mockRateProviderClient = new Mock<IRateProviderClient>();
            _mockCacheService = new Mock<ICacheService>();
            _mockTradeRepository = new Mock<ITradeRepository>();
            _mockClientRepository = new Mock<IClientRepository>();
            _mockLogger = new Mock<ILogger<ExchangeService>>();

            var configValues = new Dictionary<string, string> {
                { "Configurations:CacheTime", "30" },
                { "Configurations:TradeLimit", "10" },
                { "Configurations:HourLimit", "1" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configValues)
                .Build();

            _exchangeService = new ExchangeService(
                _mockRateProviderClient.Object,
                _mockCacheService.Object,
                _mockTradeRepository.Object,
                _mockClientRepository.Object,
                _mockLogger.Object,
                configuration
            );
        }
        [Fact]
        public async Task CreateTradeAsync_ClientDoesNotExist_ThrowsApiException() {
            // Arrange
            var tradeRequest = new TradeRequest { ID_Client = 1, FromCurrency = "USD", ToCurrency = "EUR", Amount = 100 };

            _mockClientRepository.Setup(repo => repo.ClientExistsAsync(It.IsAny<int>())).ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ApiException>(() => _exchangeService.CreateTradeAsync(tradeRequest));
            Assert.Equal(404, exception.ErrorResponse.Status);
            Assert.Equal("The Client doesn't exists", exception.Message);
        }
        public static IEnumerable<object[]> GetTradeRequests() {
            var tradeRequests = new List<object[]>();
            for(int i = 0; i < 11; i++) {
                tradeRequests.Add(new object[]
                {
                new TradeRequest
                {
                    ID_Client = i,
                    FromCurrency = "USD",
                    ToCurrency = "EUR",
                    Amount = 100 + i
                }
                });
            }
            return tradeRequests;
        }
        [Theory]
        [MemberData(nameof(GetTradeRequests))]
        public async Task CreateTradeAsync_ClientExceedsTradeLimit_ThrowsApiException(TradeRequest tradeRequest) {
            // Arrange
            var clientId = tradeRequest.ID_Client;
            _mockClientRepository.Setup(repo => repo.GetClientByIdAsync(clientId))
                .ReturnsAsync(new Client { ID = clientId });

            _mockTradeRepository.Setup(repo => repo.GetCountTradesForClientBetweenDatesAsync(clientId, It.IsAny<DateTime>(), null))
                .ReturnsAsync(clientId >= 10 ? 10 : clientId); // Ensure the limit is reached at the 10th client
            _mockRateProviderClient.Setup(x => x.GetRateAsync(tradeRequest.FromCurrency)).Throws(new ApiException(404, "", ""));
            _mockRateProviderClient.Setup(provider => provider.GetRateAsync(tradeRequest.FromCurrency))
                .ReturnsAsync(new CurrencyRate {
                    BaseCurrency = tradeRequest.FromCurrency,
                    Rates = new Dictionary<string, decimal> { { tradeRequest.ToCurrency, 0.85m } },
                    FetchedAt = DateTime.UtcNow
                });

            // Act & Assert
            if(clientId >= 10) {
                var exception = await Assert.ThrowsAsync<ApiException>(() => _exchangeService.CreateTradeAsync(tradeRequest));
                Assert.Equal(429, exception.ErrorResponse.Status);
                Assert.Equal("Reached the maximum limit of 10 trades per hour.", exception.Message);
            } else {
                var trade = await _exchangeService.CreateTradeAsync(tradeRequest);
                Assert.NotNull(trade);
                Assert.Equal(0.85m, trade.Rate);
            }
        }
    }
}
