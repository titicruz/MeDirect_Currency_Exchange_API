using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Models.DTOs;

namespace MeDirect_Currency_Exchange_API.Interfaces {
    public interface IExchangeService {
        /// <summary>
        /// Gets the exchange rate a currency pair.
        /// </summary>
        /// <param name="fromCurrency">The currency to exchange from.</param>
        /// <param name="toCurrency">The currency to exchange to.</param>
        /// <returns>The exchange rate if successful, otherwise null.</returns>
        Task<decimal?> GetRateAsync(string fromCurrency, string toCurrency);

        /// <summary>
        /// Creates a new trade based on the provided trade request.
        /// </summary>
        /// <param name="tradeRequest">The trade request containing details of the trade.</param>
        /// <returns>The created trade if successful, otherwise null.</returns>
        Task<Trade> CreateTradeAsync(TradeRequest tradeRequest);
    }
}
