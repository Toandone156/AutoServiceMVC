using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class User : Account
    {
        //Attributes
        [Key]
        public int UserId { get; set; }
        public int Point { get; set; } = 0;
        public long TotalAmount { get; set; } = 0;

        //Foreign Keys
        public int UserTypeId { get; set; }

        //Relations
        [ForeignKey("UserTypeId")]
        public virtual UserType? UserType { get; set; }
        public virtual ICollection<ServiceFeedback>? ServiceFeedbacks { get; set; }
        public virtual ICollection<PointTrading>? PointTradings { get; set; }
        public virtual ICollection<ProductFeedback>? ProductFeedbacks { get; set; }
        public virtual ICollection<UserCoupon>? UserCoupons { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<FavoriteProduct>? FavoriteProducts { get; set; }
    }
}
