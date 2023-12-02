namespace ECommerce.Models
{
	public class Receipt
	{
		public int Id { get; set; }
		public Order order { get; set; }
	}
}
