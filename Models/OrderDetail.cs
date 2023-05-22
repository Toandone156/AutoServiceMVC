using System.ComponentModel.DataAnnotations;

namespace AutoServiceMVC.Models
{
    public class OrderDetail
    {
        //Attributes
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        //Relations
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
