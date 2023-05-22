using System.ComponentModel.DataAnnotations;

namespace AutoServiceBE.Models
{
    public class OrderDetail
    {
        //Attributes
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }

        //Relations
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
