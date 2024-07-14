using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeDirect_Currency_Exchange_API.Data.Models {
    [Table("Client")]
    public class Client {
        [Key]
        public int ID { get; }
        public string Name { get; set; }
        public DateTime DT_Create { get; }
        public virtual ICollection<Trade> Trades { get; set; }

    }
}
