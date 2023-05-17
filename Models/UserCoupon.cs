using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
{
    public class UserCoupon
    {
        //Attributes
        public int UserID { get; set; }
        public int CouponID { get; set; }
        public bool IsUsed { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? ExpireAt { get; set;}

        //Relations
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        [ForeignKey("CouponID")]
        public virtual Coupon Coupon { get; set; }
    }
}
