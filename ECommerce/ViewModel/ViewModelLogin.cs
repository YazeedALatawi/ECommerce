using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModel
{
    public class ViewModelLogin
    {
        [Required, EmailAddress, MaxLength(40)]
        public string Email { get; set;}
        [Required, DataType(DataType.Password), MaxLength(50)]
        public string Password { get; set;}
        public bool RememberMe { get; set; }
    }
}
