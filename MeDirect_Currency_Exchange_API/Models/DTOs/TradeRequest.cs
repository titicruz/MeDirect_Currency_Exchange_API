namespace MeDirect_Currency_Exchange_API.Models.DTOs {
    public class TradeRequest {
        public int ID_Client { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
    }
}
