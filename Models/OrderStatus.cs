using System.ComponentModel.DataAnnotations;

namespace AutoServiceBE.Models
{
    public class OrderStatus
    {
        //Attributes
        [Key]
        public int OrderStatusId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //Foreign Key
        public int OrderID { get; set; }
        public int StatusID { get; set; }
        public int? EmployeeID { get; set; }

        //Relations
        public virtual Order? Order { get; set; }
        public virtual Status? Status { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
