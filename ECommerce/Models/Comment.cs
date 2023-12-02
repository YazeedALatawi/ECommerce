using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
	public class Comment
	{
		public int Id { get; set; }
		public User user { get; set; }
		public Product product { get; set; }
		[MinLength(3),MaxLength(60)]
		public string comment { get; set; }
	}
}
