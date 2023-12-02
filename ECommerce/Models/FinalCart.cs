using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
	public class FinalCart
	{
		public int Id { get; set; }
		public Cart cart { get; set; }
		public Product product { get; set; }
		[MinLength(1), MaxLength(30)]
		[Required]
		public int count { get; set; }
	}
}
