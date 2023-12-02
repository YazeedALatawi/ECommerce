﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ECommerce.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public Category category { get; set; }
		[Required,MaxLength(60), MinLength(1)]
		public string Name { get; set; }
		[MaxLength(300), MinLength(1)]
		public string? Description { get; set; }
		public string? MadeIn { get; set; }
		[Required]
		public int Count { get; set;}
		[Required, MaxLength(500000)]
		public double Price { get; set;}
		[MinLength(1), MaxLength(100)]
		public int? Discount { get; set;}
		public string? Image { get; set; }
		[AllowNull]
		public double? size { get; set; }
		public List<Comment> comments { get; set; }
	}
}