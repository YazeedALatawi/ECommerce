using ECommerce.Models;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModel
{
    public class ViewModelProduct
    {
        public List<Category>? categoryies { get; set; }
        [Required(ErrorMessage = "هذا الحقل اجباري")]
        public int categoryId { get; set; }
        [MinLength(3), MaxLength(50), Required(ErrorMessage = "هذا الحقل اجباري")]
        public string Name { get; set; }
        [MaxLength(200)]
        public string? Description { get; set; }
        [MaxLength(30)]
        public string? MadeIn { get; set; }
        [Required(ErrorMessage = "هذا الحقل اجباري")]
        public int Count { get; set; }
        [Required(ErrorMessage = "هذا الحقل اجباري")]
        public double Price { get; set; }
        
        public int? Discount { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        public IFormFile image { get; set; }
        
        public double? size { get; set; }

        public string? imageUrl { get; set; }
    }
}
