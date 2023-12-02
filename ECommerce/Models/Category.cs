using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
	public class Category
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "هذا الحقل اجباري")]
		public string Name { get; set; }
	}
}
