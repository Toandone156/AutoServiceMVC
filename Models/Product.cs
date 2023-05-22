using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class Product
    {
        //Attributes
        [Key]
        public int ProductId { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        [DisplayName("Name")]
        public string ProductName { get; set; }
        [DataType(DataType.Currency)]
        public int Price { get; set; }
        [Column(TypeName = "ntext")]
        [DisplayName("Decription")]
        public string ProductDescription { get; set; }
        [DataType(DataType.ImageUrl)]
        public string ProductImage { get; set; }
        public decimal ProductRating { get; set; } = 5;
        public bool IsAvailable { get; set; } = true;
        public bool IsOutOfStock { get; set; } = false;

        //Foreign Key
        public int CategoryId { get; set; }

        //Relations
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<ProductFeedback>? ProductFeedbacks { get; set; }
    }
}
