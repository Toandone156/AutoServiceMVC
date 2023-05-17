using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
{
    public class Coupon
    {
        //Attributes
        [Key]
        public int CouponID { get; set; }
        public string CouponCode { get; set; }
        public int? DiscountValue { get; set; }
        public int? DiscountPercentage { get; set; }
        public int? MinimumOrderAmount { get; set; }
        public int? MaximumDiscountAmount { get; set; }
        public bool isForNewUser { get; set; }
        public int? Quantity { get; set; }
        public int PointAmount { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartAt { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? EndAt { get; set; }

        //Foreign Keys
        public int CreatorID { get; set; }
        public int? UserTypeID { get; set; }

        //Relations
        [ForeignKey("CreatorID")]
        public virtual Employee? Creator { get; set; }
        [ForeignKey("UserTypeID")]
        public virtual UserType? UserType { get; set; }
        public virtual ICollection<UserCoupon>? UserCoupons { get; set; }
    }
}
