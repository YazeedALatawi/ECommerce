using ECommerce.Models;
using ECommerce.Models.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOperations<Order> _order;
        private readonly IOperations<Cart> _cart;
        private readonly IOperations<CartProducts> _cartProducts;
		private readonly IOperations<Product> _product;

		public OrderController(IOperations<Order> order, IOperations<Cart> cart, IOperations<CartProducts> cartProducts, IOperations<Product> product)
        {
            _order = order;
            _cart = cart;
            _cartProducts = cartProducts;
			_product = product;
		}
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var orders = _order.List().ToList();
                return View(orders);
            }

            return RedirectToAction("Index","Home");

        }

        public IActionResult Detailsproduct(int Id)
        {
            var Order = _order.Find(Id);
            var Cart = _cart.Find(Order.cartId);
            var allProducts = _cartProducts.findAllByCartId(Cart.Id);
            foreach(var item in allProducts)
            {
                item.product = _product.Find(item.productId);
            }

            return View(allProducts);

        }
    }
}
