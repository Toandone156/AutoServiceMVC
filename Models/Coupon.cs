using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class Coupon
    {
        //Attributes
        [Key]
        public int CouponId { get; set; }
        [RegularExpression("^[a-zA-Z0-9]{6,}$", ErrorMessage = "Coupon Code is not valid")]
        [StringLength(12)]
        public string CouponCode { get; set; }
        [DataType(DataType.Currency)]
        public int? DiscountValue { get; set; }
        [RegularExpression("^(?:100(?:\\.0{1,2})?|\\d{1,2}(?:\\.\\d{1,2})?)$", ErrorMessage = "Percentage is not valid")]
        public int? DiscountPercentage { get; set; }
        [DataType(DataType.Currency)]
        public int? MinimumOrderAmount { get; set; }
        [DataType(DataType.Currency)]
        public int? MaximumDiscountAmount { get; set; }
        public bool isForNewUser { get; set; }
        [RegularExpression("^\\d+$", ErrorMessage = "Quantity must a unsign number")]
        public int? Quantity { get; set; }
        [RegularExpression("^\\d+$", ErrorMessage = "PointAmount must a unsign number")]
        public int PointAmount { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartAt { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? EndAt { get; set; }

        //Foreign Keys
        public int CreatorId { get; set; }
        public int? UserTypeId { get; set; }

        //Relations
        [ForeignKey("CreatorId")]
        public virtual Employee? Creator { get; set; }
        [ForeignKey("UserTypeId")]
        public virtual UserType? UserType { get; set; }
        public virtual ICollection<UserCoupon>? UserCoupons { get; set; }
    }
}
