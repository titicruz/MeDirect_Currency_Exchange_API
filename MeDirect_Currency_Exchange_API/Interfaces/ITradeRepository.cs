using MeDirect_Currency_Exchange_API.Data.Models;

namespace MeDirect_Currency_Exchange_API.Interfaces {
    public interface ITradeRepository {
        Task AddTradeAsync(Trade trade);


        /// <summary>
        /// Gets the list of trades made by the client between the dates.
        /// </summary>
        /// <param name="id_Client">The Client ID.</param>
        /// <param name="startDate">The starting date of the search.</param>
        /// <param name="endDate">The end date of the search, ignored if null</param>
        Task<List<Trade>> GetTradesForClientBetweenDatesAsync(int id_Client, DateTime startDate, DateTime? endDate);
        /// <summary>
        /// Gets All trades from a Client by ID.
        /// </summary>
        /// <param name="id_Client">Client ID</param>
        Task<List<Trade>> GetAllTradesForClientAsync(int id_Client);
    }
}
