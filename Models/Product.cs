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
        [StringLength(255)]
        [DisplayName("Name")]
		[RegularExpression("^[a-zA-ZÀ-Ỹà-ỹĂăÂâĐđÊêÔôƠơƯư\\d]+(?:\\s+[a-zA-ZÀ-Ỹà-ỹĂăÂâĐđÊêÔôƠơƯư\\d]+)*$", ErrorMessage = "Name is not valid")]
		public string ProductName { get; set; }
        [DataType(DataType.Currency)]
        [RegularExpression("^\\d+$", ErrorMessage = "Price must a unsign number. Ex: 10000")]
        public int Price { get; set; }
        [Column(TypeName = "ntext")]
        [DisplayName("Decription")]
        public string ProductDescription { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? ProductImage { get; set; }
        public decimal ProductRating { get; set; } = 5;
        public int SellerQuantity { get; set; } = 0;
        public bool IsAvailable { get; set; } = true;
        public bool IsInStock { get; set; } = true;

        //Foreign Key
        public int? CategoryId { get; set; }

        //Relations
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
        public virtual ICollection<ProductFeedback>? ProductFeedbacks { get; set; }
        public virtual ICollection<FavoriteProduct>? FavoriteProducts { get; set; }

        //Not map attributes
        [NotMapped]
        public bool? Favorite { get; set; }
    }
}
