using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Models
{
	public class Shipping
	{
		[Key]
		public int Id { get; set; }
		[System.ComponentModel.DataAnnotations.Required]
		public string Name { get; set; }
		public string? image { get; set; }
	}
}
