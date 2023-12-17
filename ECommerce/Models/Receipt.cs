using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Receipt
	{
		public int Id { get; set; }
        public int orderId { get; set; }

        [ForeignKey("orderId")]
        public Order order { get; set; }
	}
}
