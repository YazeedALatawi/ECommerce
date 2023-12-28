using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class CartProducts
	{
		public int Id { get; set; }
        public int cartId { get; set; }

        [ForeignKey("cartId")]
        public Cart cart { get; set; }
        public int productId { get; set; }

        [ForeignKey("productId")]
        public Product product { get; set; }
		[MinLength(1), MaxLength(30)]
		[Required]
		public int count { get; set; }
		public List<CartPrdouctOptions> options { get; set; }
		[NotMapped]
		public string key { get; set; }
	}
}
