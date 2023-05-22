using System.ComponentModel.DataAnnotations;

namespace AutoServiceMVC.Models
{
    public class OrderStatus
    {
        //Attributes
        [Key]
        public int OrderStatusId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //Foreign Key
        public int OrderId { get; set; }
        public int StatusId { get; set; }
        public int? EmployeeId { get; set; }

        //Relations
        public virtual Order? Order { get; set; }
        public virtual Status? Status { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
