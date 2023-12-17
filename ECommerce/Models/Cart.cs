using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Cart
	{
		public int Id { get; set; }
        public string userId { get; set; }

        [ForeignKey("userId")]
        public User user { get; set; }
		public bool isDelete { get; set; }
		public double totalPrice { get; set; }
	}
}
