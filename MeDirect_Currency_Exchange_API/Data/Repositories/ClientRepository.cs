using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Services;
using Microsoft.EntityFrameworkCore;

namespace MeDirect_Currency_Exchange_API.Data.Repositories {
    public class ClientRepository : IClientRepository {
        private Currency_Exchange_API_Context _context;
        private ICacheService _cachService;

        public ClientRepository(Currency_Exchange_API_Context context,ICacheService cacheService) {
            _context = context;
            _cachService = cacheService;
        }
        public async Task AddClientAsync(Client client) {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Client>> GetAllClientsAsync() {
            return await _context.Clients.ToListAsync();
        }

        public async Task<bool> ClientExistsAsync(int id_Client) {
            string cacheKey = $"CExists_{id_Client}";
            bool? exists = _cachService.Get<bool>(cacheKey);
            if(exists != null)
                return exists.Value;
            exists = await _context.Clients.AnyAsync(c => c.ID == id_Client);
            if (exists.Value)
                _cachService.Set<bool>(cacheKey, exists.Value,null);
            return exists.Value;
        }
        public async Task<Client?> GetClientByIdAsync(int id_Client) {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.ID == id_Client);
        }

        public async Task UpdateClientAsync(Client client) {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }
    }
}
