using MeDirect_Currency_Exchange_API.Data.Models;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MeDirect_Currency_Exchange_API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase {
        private IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository) {
            _clientRepository = clientRepository;
        }
        [HttpPost("AddClient")]
        public async Task<IActionResult> AddClientAsync([FromBody] ClientRequest clientRequest) {
            var client = new Client {
                ID = clientRequest.ID,
                Name = clientRequest.Name,
                DT_Create = clientRequest.DT_Create
            };
            await _clientRepository.AddClientAsync(client);
            return Ok("Client inserted");
        }
        [HttpGet("GetClient")]
        public async Task<IActionResult> GetClientAsync(int id_Client) {
            Client client = await _clientRepository.GetClientByIdAsync(id_Client);
            if(client == null) {
                return NotFound($"Client with ID {id_Client} not found.");
            }
            return Ok(client);
        }
    }
}
