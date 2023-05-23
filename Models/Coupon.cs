using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
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
        [DisplayName("Coupon code")]
        public string CouponCode { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Discount value")]
        public int? DiscountValue { get; set; }
        [RegularExpression("^(?:100(?:\\.0{1,2})?|\\d{1,2}(?:\\.\\d{1,2})?)$", ErrorMessage = "Percentage is not valid")]
        [DisplayName("Discount percentage")]
        public int? DiscountPercentage { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Minimum order")]
        public int? MinimumOrderAmount { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Maximum discount")]
        public int? MaximumDiscountAmount { get; set; }
        [DisplayName("For new user")]
        public bool isForNewUser { get; set; }
        [RegularExpression("^\\d+$", ErrorMessage = "Quantity must a unsign number")]
        public int? Quantity { get; set; }
        [RegularExpression("^\\d+$", ErrorMessage = "PointAmount must a unsign number")]
        [DisplayName("Point amount")]
        public int PointAmount { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayName("Start date")]
        public DateTime StartAt { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayName("End date")]
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
