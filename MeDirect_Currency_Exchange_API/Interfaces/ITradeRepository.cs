using MeDirect_Currency_Exchange_API.Data.Models;

namespace MeDirect_Currency_Exchange_API.Interfaces {
    public interface ITradeRepository {
        Task AddTradeAsync(Trade trade);
        Task<List<Trade>> GetTradesForClientBetweenDatesAsync(string clientId, DateTime startDate, DateTime endDate);
        Task<List<Trade>> GetAllTradesForClientAsync(string clientId);
    }
}
