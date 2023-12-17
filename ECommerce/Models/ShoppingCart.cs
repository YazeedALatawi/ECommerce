using ECommerce.ViewModel;

namespace ECommerce.Models
{
    public class ShoppingCart
    {
        public List<ProductCart> Products { get; set; } = new List<ProductCart>();
        public double totalPrice { get; set; }

    }
}
