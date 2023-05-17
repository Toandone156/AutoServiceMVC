using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
{
    public class ProductFeedback
    {
        //Attributes
        [Key]
        public int ProductFeedbackID { get; set; }
        [Column(TypeName = "ntext")]
        public string Comment { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? Image { get; set; }
        [Column(TypeName = "tinyint")]
        public int Rating { get; set; }

        //Foreign keys
        public int ProductID { get; set; }
        public int UserID { get; set; }

        //Relations
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
