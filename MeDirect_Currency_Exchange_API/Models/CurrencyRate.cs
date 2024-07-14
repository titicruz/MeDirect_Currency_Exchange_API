namespace MeDirect_Currency_Exchange_API.Models {
    public class CurrencyRate {
        public string BaseCurrency { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
        public DateTime FetchedAt { get; set; }
    }
}
