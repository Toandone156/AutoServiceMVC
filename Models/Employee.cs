using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
{
    public class Employee : Account
    {
        //Attributes
        [Key]
        public int EmployeeID { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        //Relation
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Order> CreatedOrders { get; set; }
        public virtual ICollection<Coupon> CreatedCoupons { get; set; }
    }
}
