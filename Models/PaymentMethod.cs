using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
{
    public class PaymentMethod
    {
        //Attributes
        [Key]
        public int PaymentMethodId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string PaymentMethodName { get; set;}

        //Relations
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
