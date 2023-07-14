using System.ComponentModel.DataAnnotations.Schema;

namespace AutoServiceMVC.Models
{
    public class FavoriteProduct
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
