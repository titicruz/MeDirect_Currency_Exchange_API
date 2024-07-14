using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeDirect_Currency_Exchange_API.Data.Models {
    [Table("Trade")]
    public class Trade {
        [Key]
        public int ID { get; }
        [ForeignKey("Client")]
        public int ID_Client { get; set; }
        public virtual Client Client { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal ExchangeRate { get; set; }
        public DateTime Dt_Create { get; set; }
    }
}
