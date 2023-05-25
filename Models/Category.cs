using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceMVC.Models
{
    public class Category
    {
        //Attributes
        [Key]
        public int CategoryId { get; set; }
        [StringLength(100)]
        [DisplayName("Category")]
        public string CategoryName { get; set; }

        //Relations
        public virtual ICollection<Product>? Products { get; set; }
    }
}
