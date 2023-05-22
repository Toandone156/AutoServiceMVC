using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class UserType
    {
        //Attributes
        [Key]
        public int UserTypeId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string UserTypeName { get; set; }
        public int MinAmount { get; set; }

        //Relations
        public virtual ICollection<User>? Users { get; set; }
        public virtual ICollection<Coupon>? Coupons { get; set; }
    }
}
