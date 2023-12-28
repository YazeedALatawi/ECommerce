using ECommerce.Models;

namespace ECommerce.ViewModel
{
    public class orderAdminViewModel
    {
        public int Id { get; set; }
        public List<CartProducts> cartProduct { get; set; }
        public string shipping { get; set; }
        public string orderState { get; set;}

        public User User { get; set; }
        public List<Shipping> shippingList { get; set; }

        public List<string> orderStatesList = new List<string>
        {
            "جديد",
            "جاري التجهيز",
            "تم الشحن",
            "تم الاستلام من العميل",
            "ملغي"
        };
    }
}
