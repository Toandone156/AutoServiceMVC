using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceBE.Models
{
    public class Role
    {
        //Attributes
        [Key]
        public int RoleId { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string RoleName { get; set; }

        //Relations
        public virtual ICollection<Employee>? Employees { get; set; }
    }
}
