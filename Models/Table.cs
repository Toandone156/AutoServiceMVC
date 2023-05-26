using System.ComponentModel;
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
        [DisplayName("Table name")]
        public string TableName { get; set; }
        [StringLength(100)]
        [RegularExpression("^[a-zA-Z0-9]{6,30}$", ErrorMessage = "Table Code is not valid")]
        [DisplayName("Table code")]
        public string TableCode { get; set; }

        //Relations
        public virtual ICollection<Order>? Orders { get; set; }
    }
}
