using System.ComponentModel.DataAnnotations;

namespace MeDirect_Currency_Exchange_API.Models.DTOs {
    public class TradeRequest {
        [Required]
        public int ID_Client { get; set; }
        [Required]
        public string FromCurrency { get; set; }
        [Required]
        public string ToCurrency { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
    }
}
