using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeDirect_Currency_Exchange_API.Data.Repositories {
    public class TradeRepository : ITradeRepository {
        private Currency_Exchange_API_Context _context;
        public TradeRepository(Currency_Exchange_API_Context context) {
            _context = context;
        }
        public async Task AddTradeAsync(Trade trade) {
            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Trade>> GetAllTradesForClientAsync(int id_Client) {
            return await _context.Trades.Where(t=> t.ID_Client == id_Client).ToListAsync();
        }
        
        public async Task<List<Trade>> GetTradesForClientBetweenDatesAsync(int id_Client, DateTime startDate, DateTime? endDate = null) {
            if (endDate.HasValue) {
                return await _context.Trades.Where(t => t.ID_Client == id_Client && t.Dt_Create >= startDate && t.Dt_Create <= endDate).ToListAsync();
            } else {
                return await _context.Trades.Where(t => t.ID_Client == id_Client && t.Dt_Create >= startDate).ToListAsync();
            }
        }
    }
}
