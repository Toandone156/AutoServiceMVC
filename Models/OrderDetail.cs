using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AutoServiceMVC.Models
{
	[JsonObject(MemberSerialization.OptIn)]
	public class OrderDetail
    {
        //Attributes
        [JsonProperty]
        public int OrderId { get; set; }
		[JsonProperty]
		public int ProductId { get; set; }
        [DataType(DataType.Currency)]
		[JsonProperty]
		public int Price { get; set; }
		[JsonProperty]
		public int Quantity { get; set; }

        //Relations
        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
