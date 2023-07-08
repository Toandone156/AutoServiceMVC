using AutoServiceMVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Coupon
    {
        //Attributes
        [Key]
		[JsonProperty]
		public int CouponId { get; set; }
        [RegularExpression("^[a-zA-Z0-9]{6,30}$", ErrorMessage = "Coupon Code is not valid")]
        [StringLength(30)]
        [DisplayName("Coupon code")]
		[JsonProperty]
		public string CouponCode { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Discount value")]
        [RegularExpression("^\\d+$", ErrorMessage = "Discount value must a unsign number")]
        [JsonProperty]
		public int? DiscountValue { get; set; }
        [DisplayName("Discount percentage")]
        [RegularExpression("^(?:100(?:\\.0{1,2})?|\\d{1,2}(?:\\.\\d{1,2})?)$", ErrorMessage = "Percentage value is not valid")]
		[JsonProperty]
		public int? DiscountPercentage { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Minimum order")]
        [RegularExpression("^\\d+$", ErrorMessage = "Min Order must a unsign number")]
        [JsonProperty]
		public int? MinimumOrderAmount { get; set; }
        [DataType(DataType.Currency)]
        [DisplayName("Maximum discount")]
        [RegularExpression("^\\d+$", ErrorMessage = "Max Discount must a unsign number")]
        [JsonProperty]
		public int? MaximumDiscountAmount { get; set; }
		[RegularExpression("^\\d+$", ErrorMessage = "Quantity must a unsign number")]
		public int? Quantity { get; set; }
		[RegularExpression("^\\d+$", ErrorMessage = "Remain must a unsign number")]
		public int? Remain { get; set; }
        [RegularExpression("^\\d+$", ErrorMessage = "Point must a unsign number")]
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
