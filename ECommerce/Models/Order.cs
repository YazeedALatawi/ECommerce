using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Order
	{
		public int Id { get; set; }
        public int cartId { get; set; }

        [ForeignKey("cartId")]
        public Cart cart { get; set; }
		public double totalPrice { get; set; }
		public string orderState { get; set; }
		public DateTime OrderDate { get; set; }
        public string shipping { get; set; }


	}
}
