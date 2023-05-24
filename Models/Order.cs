using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class Order
    {
        //Attributes
        [Key]
        public int OrderId { get; set; }
        public int Amount { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column(TypeName = "ntext")]
        public string Note { get; set; }

        //Foreign Keys
        public int? UserId { get; set; }
        public int? EmployeeId { get; set; }
        public int TableId { get; set; }
        public int? ApplyCouponId { get; set; }
        public int PaymentMethodId { get; set; }

        //Relations
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }
        [ForeignKey("TableId")]
        public virtual Table? Table { get; set; }
        [ForeignKey("ApplyCouponId")]
        public virtual Coupon? ApplyCoupon { get; set; }
        [ForeignKey("PaymentMethodId")]
        public virtual PaymentMethod? PaymentMethod { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<OrderStatus>? OrderStatuses { get; set; }

        //Not map attribute
        [NotMapped]
        public Status? Status { get; set; }
    }
}
