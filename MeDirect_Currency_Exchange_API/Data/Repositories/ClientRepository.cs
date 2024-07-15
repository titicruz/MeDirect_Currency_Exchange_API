using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MeDirect_Currency_Exchange_API.Data.Repositories {
    public class ClientRepository : IClientRepository {
        private Currency_Exchange_API_Context _context;

        public ClientRepository(Currency_Exchange_API_Context context) { 
            _context = context;
        }
        public async Task AddClientAsync(Client client) {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Client>> GetAllClientsAsync() {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client?> GetClientByIdAsync(int id_client) {
            return await _context.Clients
                .Include(c=>c.Trades)
                .FirstOrDefaultAsync(c=>c.ID == id_client);
        }

        public async Task UpdateClientAsync(Client client) {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
        }
    }
}
