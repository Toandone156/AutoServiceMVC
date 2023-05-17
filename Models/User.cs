using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
{
    public class User : Account
    {
        //Attributes
        [Key]
        public int UserId { get; set; }
        public int Point { get; set; }

        //Foreign Keys
        public int UserTypeID { get; set; }

        //Relations
        [ForeignKey("UserTypeID")]
        public virtual UserType UserType { get; set; }
        public virtual ICollection<ServiceFeedback> ServiceFeedbacks { get; set; }
        public virtual ICollection<PointTrading> PointTradings { get; set; }
        public virtual ICollection<ProductFeedback> ProductFeedbacks { get; set; }
        public virtual ICollection<UserCoupon> UserCoupons { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
