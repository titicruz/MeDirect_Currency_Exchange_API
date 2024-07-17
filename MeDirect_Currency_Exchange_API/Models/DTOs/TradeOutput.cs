namespace MeDirect_Currency_Exchange_API.Models.DTOs {
    public class TradeOutput {
        public int ID { get; set; }
        public int ID_Client { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Rate { get; set; }
        public decimal ExchangedAmount { get; set; }
    }
}
