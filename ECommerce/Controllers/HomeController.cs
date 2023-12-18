using ECommerce.Models;
using ECommerce.Models.Repositories;
using ECommerce.Models.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Diagnostics;

namespace ECommerce.Controllers
{
	public class HomeController : Controller
	{
        private readonly IOperations<Product> _products;
        private readonly ILogger<HomeController> _logger;
		private readonly IToastNotification _ToastNotification;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly IOperations<Cart> _cart;
        private readonly UserManager<User> _userManager;
        private readonly IOperations<CartProducts> _cartProducts;
        public HomeController(ILogger<HomeController> logger, IToastNotification toastNotification, IOperations<Product> product, ShoppingCartService shoppingCartService, IOperations<Cart> carts, UserManager<User> userManager, IOperations<CartProducts> cartProducts)
		{
			_logger = logger;
			_ToastNotification = toastNotification;
			_products= product;
			_shoppingCartService= shoppingCartService;
            _cart = carts;
            _userManager= userManager;
            _cartProducts= cartProducts;

        }

		public IActionResult Index()
		{
			var products = _products.List().ToList();
            ChecktheSession();
            return View(products);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


        public void ChecktheSession()
         {
             if (User.Identity.IsAuthenticated)
             {
                 var productsSession = _shoppingCartService.GetShoppingCart();
                 if (productsSession == null || productsSession.Products.Count == 0)
                 {

                 }
                 else
                 {
                     var theCart = _cart.findByIdUser(_userManager.GetUserId(User));
                     if (theCart == null)
                     {
                         var newCart = new Cart
                         {
                             userId = _userManager.GetUserId(User)
                         };
                         _cart.Add(newCart);
                         var Thecart = _cart.findByIdUser(_userManager.GetUserId(User));
                         foreach (var item in productsSession.Products)
                         {
                             _cartProducts.Add(new CartProducts
                             {
                                 cartId = Thecart.Id,
                                 productId = item.Id,
                                 count = item.Quantity,
                             });

                         }

                         _shoppingCartService.ClearSession();

                     }
                     else
                     {
                         var Thecart = _cart.findByIdUser(_userManager.GetUserId(User));
                         var alltheProductInCart = _cartProducts.findAllByCartId(theCart.Id);

                         foreach (var item in productsSession.Products)
                         {
                             var isFound = _cartProducts.findByIdProduct(item.Id);
                             if (!isFound)
                             {
                                 _cartProducts.Add(new CartProducts
                                 {
                                     cartId = Thecart.Id,
                                     productId = item.Id,
                                     count = item.Quantity,
                                 });
                             }

                         }


                         _shoppingCartService.ClearSession();
                     }
                 }
             }


         }

    }
}