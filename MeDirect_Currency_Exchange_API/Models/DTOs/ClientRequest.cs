using System.ComponentModel.DataAnnotations;

namespace MeDirect_Currency_Exchange_API.Models.DTOs {
    public class ClientRequest {
        [Required]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
