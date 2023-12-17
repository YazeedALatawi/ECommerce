using System.ComponentModel.DataAnnotations;

namespace ECommerce.ViewModel
{
    public class CheckOutViewModel
    {
        public int CartId { get; set; }
        public string firstName { get; set; }
        public string LastName { get; set; }

        [MinLength(3), MaxLength(15), Required(ErrorMessage = "هذا الحقل اجباري")]
        public string City { get; set; }

        [MinLength(3), MaxLength(15), Required(ErrorMessage = "هذا الحقل اجباري")]
        public string District { get; set; }

        [MinLength(3), MaxLength(15), Required(ErrorMessage = "هذا الحقل اجباري")]
        public string Street { get; set; }
        public double totalPrice { get; set; }
        public double shippingCost { get; set; }

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
