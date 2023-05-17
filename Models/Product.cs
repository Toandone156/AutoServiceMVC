using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
{
    public class Product
    {
        //Attributes
        [Key]
        public int ProductId { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string ProductName { get; set; }
        [DataType(DataType.Currency)]
        public int Price { get; set; }
        [Column(TypeName = "ntext")]
        public string ProductDescription { get; set; }
        public decimal ProductRating { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsOutOfStock { get; set; } = false;

        //Foreign Key
        public int CategoryID { get; set; }

        //Relations
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductFeedback> ProductFeedbacks { get; set; }
    }
}
