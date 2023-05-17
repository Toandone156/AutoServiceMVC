using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
{
    public class Status
    {
        //Attributes
        [Key]
        public int StatusId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string StatusName { get; set; }

        //Relations
        public virtual ICollection<OrderStatus> OrderStatuses { get; set;}
    }
}
