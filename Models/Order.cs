using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
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
        public int? UserID { get; set; }
        public int? EmployeeID { get; set; }
        public int TableID { get; set; }
        public int? ApplyCouponID { get; set; }
        public int PaymentMethodID { get; set; }

        //Relations
        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
        [ForeignKey("EmployeeID")]
        public virtual Employee? Employee { get; set; }
        [ForeignKey("TableID")]
        public virtual Table Table { get; set; }
        [ForeignKey("ApplyCouponID")]
        public virtual Coupon? ApplyCoupon { get; set; }
        [ForeignKey("PaymentMethodID")]
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<OrderStatus> OrderStatuses { get; set; }
    }
}
