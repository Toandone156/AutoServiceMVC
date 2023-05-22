using System.ComponentModel.DataAnnotations;

namespace AutoServiceBE.Models
{
    public class Category
    {
        //Attributes
        [Key]
        public int CategoryId { get; set; }
        [StringLength(100)]
        public string CategoryName { get; set; }

        //Relations
        public virtual ICollection<Product>? Products { get; set; }
    }
}
