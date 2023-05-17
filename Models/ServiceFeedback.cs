using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
{
    public class ServiceFeedback
    {
        //Attributes
        [Key]
        public int ServiceFeedbackId { get; set; }
        [Column(TypeName = "ntext")]
        public string Comment { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? Image { get; set; }

        //Foreign Keys
        public int UserID { get; set; }

        //Relations
        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
    }
}
