using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class ProductFeedback
    {
        //Attributes
        [Key]
        public int ProductFeedbackId { get; set; }
        [Column(TypeName = "ntext")]
        public string? Comment { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? Image { get; set; }
        [Column(TypeName = "tinyint")]
        public int Rating { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

		//Foreign keys
		public int ProductId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }

        //Relations
        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
        public virtual Order? Order { get; set; }
    }
}
