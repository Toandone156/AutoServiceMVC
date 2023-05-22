using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceBE.Models
{
    public class PointTrading
    {
        //Attributes
        [Key]
        public int PointTradingId { get; set; }
        public int Point { get; set; }
        [Column(TypeName = "ntext")]
        public string TradeDescription { get; set; }
        [DataType(DataType.Date)]
        public DateTime TradedAt { get; set; } = DateTime.Now;

        //Foreign Keys
        public int UserID { get; set; }

        //Relations
        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
    }
}
