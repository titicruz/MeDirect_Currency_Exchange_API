using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MeDirect_Currency_Exchange_API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase {
        private IClientRepository _clientRepository;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IClientRepository clientRepository, ILogger<ClientController> logger) {
            _clientRepository = clientRepository;
            _logger = logger;
        }
        [HttpPost("AddClient")]
        public async Task<IActionResult> AddClientAsync([FromBody] ClientRequest clientRequest) {
            _logger.LogInformation($"Adding new client name:{clientRequest.Name}");
            var client = new Client {
                ID = clientRequest.ID,
                Name = clientRequest.Name,
                DT_Create = DateTime.Now
            };
            await _clientRepository.AddClientAsync(client);
            _logger.LogInformation($"Client name:{clientRequest.Name} added!");
            return Ok("Client inserted");
        }
        [HttpGet("GetClient")]
        public async Task<IActionResult> GetClientAsync(int id_Client) {
            _logger.LogInformation($"Getting client by ID:{id_Client}");
            var client = await _clientRepository.GetClientByIdAsync(id_Client);
            if(client == null) {
                return NotFound($"Client with ID {id_Client} not found.");
            }
            _logger.LogInformation($"Client Found name:{client.Name}");
            return Ok(client);
        }
    }
}
