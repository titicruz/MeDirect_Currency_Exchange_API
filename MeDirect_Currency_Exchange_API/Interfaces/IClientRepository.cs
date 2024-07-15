using MeDirect_Currency_Exchange_API.Data.Models;

namespace MeDirect_Currency_Exchange_API.Interfaces {
    public interface IClientRepository {

        /// <summary>
        /// Adds a Client to the Database.
        /// </summary>
        /// <param name="client">Client info to insert</param>
        Task AddClientAsync(Client client);
        /// <summary>
        /// Gets Client by ID.
        /// </summary>
        /// <param name="id_Client">Client ID</param>
        Task<Client?> GetClientByIdAsync(int id_Client);
        /// <summary>
        /// Gets All Clients.
        /// </summary>
        /// <param name="id_Client">Client ID</param>
        Task<List<Client>> GetAllClientsAsync();
        /// <summary>
        /// Updates a Clients.
        /// </summary>
        /// <param name="client">Client info to update</param>
        Task UpdateClientAsync(Client client);
    }
}
