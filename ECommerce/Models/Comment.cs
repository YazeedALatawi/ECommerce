using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Models
{
	public class Comment
	{
		public int Id { get; set; }
        public string userId { get; set; }

        [ForeignKey("userId")]
        public User user { get; set; }
        public int productId { get; set; }

        [ForeignKey("productId")]
        public Product product { get; set; }
		[MinLength(3),MaxLength(60)]
		public string comment { get; set; }
	}
}
