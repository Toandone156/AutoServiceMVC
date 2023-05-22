using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class Employee : Account
    {
        //Attributes
        [Key]
        public int EmployeeId { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        //Foreign Keys
        public int RoleId { get; set; }

        //Relations
        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
        public virtual ICollection<Order>? CreatedOrders { get; set; }
        public virtual ICollection<Coupon>? CreatedCoupons { get; set; }
    }
}
