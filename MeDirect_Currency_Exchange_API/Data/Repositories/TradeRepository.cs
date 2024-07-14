using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Interfaces;

namespace MeDirect_Currency_Exchange_API.Data.Repositories {
    public class TradeRepository : ITradeRepository {
        public TradeRepository() {

        }
        public Task AddTradeAsync(Trade trade) {
            throw new NotImplementedException();
        }

        public Task<List<Trade>> GetAllTradesForClientAsync(string clientId) {
            throw new NotImplementedException();
        }

        public Task<List<Trade>> GetTradesForClientBetweenDatesAsync(string clientId, DateTime startDate, DateTime endDate) {
            throw new NotImplementedException();
        }
    }
}
