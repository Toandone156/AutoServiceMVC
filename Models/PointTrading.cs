using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceMVC.Models
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
        public int UserId { get; set; }

        //Relations
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
