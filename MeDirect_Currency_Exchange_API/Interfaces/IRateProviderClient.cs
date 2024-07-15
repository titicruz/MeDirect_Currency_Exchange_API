using MeDirect_Currency_Exchange_API.Models;

namespace MeDirect_Currency_Exchange_API.Interfaces {
    public interface IRateProviderClient {
        /// <summary>
        /// Gets the exchange rates from a currency.
        /// </summary>
        /// <param name="Currency">The currency to exchange from.</param>
        /// <returns>The list of exchange rates if successful, otherwise null.</returns>
        Task<CurrencyRate> GetRateAsync(string Currency);
    }
}
