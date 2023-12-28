using ECommerce.Models;
using ECommerce.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace ECommerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOperations<Order> _order;
        private readonly UserManager<User> userManager;
        private readonly IOperations<Cart> _cart;
        private readonly IOperations<CartProducts> _cartProducts;
		private readonly IOperations<Product> _product;
        private readonly IOperations<CartPrdouctOptions> _cartProductOptions;
        private readonly IDataProtector _dataProtector;
        public OrderController(IOperations<Order> order,UserManager<User> userManager, IOperations<Cart> cart, IOperations<CartProducts> cartProducts, IOperations<Product> product, IOperations<CartPrdouctOptions> cartProductOptions, IDataProtectionProvider dataProtectionProvider)
        {
            _order = order;
            this.userManager = userManager;
            _cart = cart;
            _cartProducts = cartProducts;
			_product = product;
            _cartProductOptions = cartProductOptions;
            _dataProtector = dataProtectionProvider.CreateProtector("ECommerce");
        }
        [Authorize]
        public async Task<IActionResult> Index(int? page)
        {
            if (User.Identity.IsAuthenticated)
            {

                var userID = userManager.GetUserId(User);
                var AllCart = _cart.List().Where(a => a.userId == userID).ToList();
                var matchingOrders = _order.List().Where(order => AllCart.Any(cartItem => cartItem.Id == order.cartId)).ToList();



                var orders = matchingOrders.ToList().OrderByDescending(i => i.Id).ToPagedList(page ?? 1, 10);
                return View(orders);
            }

            return RedirectToAction("Index","Home");

        }

        [Authorize]
        public IActionResult Detailsproduct(int Id)
        {
            var Order = _order.Find(Id);
            var Cart = _cart.Find(Order.cartId);
            var UserID = userManager.GetUserId(User);
            if(Cart.userId != UserID)
            {
                return RedirectToAction("error","home");
            }
            var allProducts = _cartProducts.findAllByCartId(Cart.Id);
            foreach(var item in allProducts)
            {
                item.product = _product.Find(item.productId);
                item.options = _cartProductOptions.List().Where(a => a.cartID == Cart.Id && a.ProductID == item.productId).ToList();
                
            }

            allProducts.OrderByDescending(a => a.Id).ToList();
            allProducts.ToList().ForEach(a => a.key = _dataProtector.Protect(a.productId.ToString()));
            return View(allProducts);

        }
    }
}
