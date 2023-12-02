using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ECommerce.Models
{
	public class User : IdentityUser
	{

		[MinLength(3), MaxLength(18), Required]
		public string FirstName { get; set; }

		[MinLength(3), MaxLength(18), Required]
		public string LastName { get; set; }

		[MinLength(3), MaxLength(18), Required]
		public string City { get; set; }

		[MinLength(3), MaxLength(18), Required]
		public string District { get; set; }

		[MinLength(3), MaxLength(18), Required]
		public string Street { get; set; }
	}
}
