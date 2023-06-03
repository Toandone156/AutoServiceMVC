using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class UserCoupon
    {
        //Attributes
        public int UserId { get; set; }
        public int CouponId { get; set; }
        public bool IsUsed { get; set; }

        //Relations
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        [ForeignKey("CouponId")]
        public virtual Coupon? Coupon { get; set; }
    }
}
