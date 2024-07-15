using MeDirect_Currency_Exchange_API.Data.Models;

namespace MeDirect_Currency_Exchange_API.Interfaces
{
    public interface IClientRepository {
        Task AddClientAsync(Client client);
        Task<Client?> GetClientByIdAsync(int clientId);
        Task<List<Client>> GetAllClientsAsync();
        Task UpdateClientAsync(Client client);
    }
}
