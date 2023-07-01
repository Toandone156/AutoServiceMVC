using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class ServiceFeedback
    {
        //Attributes
        [Key]
        public int ServiceFeedbackId { get; set; }
        [Column(TypeName = "ntext")]
        [RegularExpression("^(?!^\\s+$).+$", ErrorMessage = "Feedback can not empty")]
        public string Comment { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? Image { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //Foreign Keys
        public int UserId { get; set; }

        //Relations
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
