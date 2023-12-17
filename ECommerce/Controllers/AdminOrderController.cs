using ECommerce.Models.Repositories;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerce.ViewModel;

namespace ECommerce.Controllers
{
    [Route("admin/Order/[action]/{id?}")]
    public class AdminOrderController : Controller
    {
        private readonly IOperations<Order> _order;
        private readonly IOperations<Cart> _cart;
        private readonly IOperations<CartProducts> _cartProducts;
        private readonly IOperations<Product> _product;
        private readonly IOperations<Shipping> _shipping;

        public AdminOrderController(IOperations<Order> order, IOperations<Cart> cart, IOperations<CartProducts> cartProducts, IOperations<Product> product, IOperations<Shipping> shipping)
        {
            _order = order;
            _cart = cart;
            _cartProducts = cartProducts;
            _product = product;
            _shipping = shipping;
        }
        public IActionResult Index()
        {
            var orders = _order.List();
            return View(orders);
        }
        [HttpGet]
        public IActionResult Details(int Id) 
        {

            var Order = _order.Find(Id);
            var Cart = _cart.Find(Order.cartId);
            var allProducts = _cartProducts.findAllByCartId(Cart.Id);
            foreach (var item in allProducts)
            {
                item.product = _product.Find(item.productId);
            }
            var model = new orderAdminViewModel
            {
                Id = Order.Id,
                cartProduct = (List<CartProducts>)allProducts,
                orderState = Order.orderState,
                shipping = Order.shipping,
                shippingList = (List<Shipping>)_shipping.List()
            };
           
            return View(model);
        }

        [HttpPost]
        public IActionResult Details(orderAdminViewModel model)
        {
            var order = _order.Find(model.Id);
            order.shipping = model.shipping;
            order.orderState = model.orderState;
            _order.update(order);
            return RedirectToAction("index");
        }
    }
}
