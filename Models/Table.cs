using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class Table
    {
        //Attributes
        [Key]
        public int TableId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string TableName { get; set; }
        [StringLength(100)]
        [RegularExpression("^[a-zA-Z0-9]{6,}$")]
        public string TableCode { get; set; }

        //Relations
        public virtual Order? Order { get; set; }
    }
}
