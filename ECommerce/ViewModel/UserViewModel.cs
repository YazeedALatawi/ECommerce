using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModel
{
    public class UserViewModel
    {
        public string id { get; set; }

        [MinLength(3), MaxLength(18), Required(ErrorMessage = "هذا الحقل اجباري")]
        public string FirstName { get; set; }

        [MinLength(3), MaxLength(18), Required(ErrorMessage = "هذا الحقل اجباري")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(70, ErrorMessage = "يجب ان يتكون الايميل من 5 - 70 حرف", MinimumLength = 5)]
        [RegularExpression("^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$", ErrorMessage = "الايميل ليس صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(60, ErrorMessage = "يجب ان تتكون كلمة السر من 5 - 60 حرف", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(60, ErrorMessage = "يجب ان تتكون كلمة السر من 5 - 60 حرف", MinimumLength = 5)]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(60, ErrorMessage = "يجب ان تتكون كلمة السر من 5 - 60 حرف", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Compare("RePassword", ErrorMessage = "غير مطابق لكلمة السر")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        [StringLength(10, ErrorMessage = "يجب ان يتكون رقم الهاتف من 10 ارقام", MinimumLength = 10)]
        [DataType(DataType.PhoneNumber, ErrorMessage = "ادخل رقم صحيح")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "هذا الحقل اجباري")]
        //[StringLength(30, ErrorMessage = "يجب ان يتكون اسم المدينة من 2 - 30 حرف", MinimumLength = 2)]
        public string City { get; set; }

        [MinLength(3), MaxLength(18), Required(ErrorMessage = "هذا الحقل اجباري")]
        public string District { get; set; }

        [MinLength(3), MaxLength(18), Required(ErrorMessage = "هذا الحقل اجباري")]
        public string Street { get; set; }


        public List<string> AvailableCities = new List<string>
        {
            "الرياض",
            "المدينة المنورة",
            "تبوك",
            "جدة",
            "الطائف",
            "الخبر",
            "مكة",
            "ضباء",
            "ابها",
            "ينبع",
            "بريدة",
            "حائل",
            "عنيزة",
            "خميس مشيط",
            "جازان"
        };
    }
}
