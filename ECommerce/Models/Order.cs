namespace ECommerce.Models
{
	public class Order
	{
		public int Id { get; set; }
		public Cart cart { get; set; }
		public double totalPrice { get; set; }
		public string orderState { get; set; }
		public DateTime OrderDate { get; set; }
		public Shipping shipping { get; set; }

	}
}
