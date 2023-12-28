using ECommerce.Models;
using ECommerce.Models.Repositories;
using ECommerce.Models.Service;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NToastNotify;
using System.Diagnostics;
using X.PagedList;

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
        private readonly IDataProtector _dataProtector;
        public HomeController(ILogger<HomeController> logger, IToastNotification toastNotification, IOperations<Product> product, ShoppingCartService shoppingCartService, IOperations<Cart> carts, UserManager<User> userManager, IOperations<CartProducts> cartProducts, IDataProtectionProvider dataProtectionProvider)
		{
			_logger = logger;
			_ToastNotification = toastNotification;
			_products= product;
			_shoppingCartService= shoppingCartService;
            _cart = carts;
            _userManager= userManager;
            _cartProducts = cartProducts;
            _dataProtector = dataProtectionProvider.CreateProtector("ECommerce");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]


        public IActionResult NoAuthenticated(string? ReturnUrl)
        {
            if (ReturnUrl != null)
            {
                _ToastNotification.AddErrorToastMessage("يجب عليك تسجيل الدخول");
                return RedirectToAction("index");
            }

            return RedirectToAction("index");

        }
        public IActionResult AccessDenied(string? ReturnUrl)
        {
            if(ReturnUrl != null)
            {
                return RedirectToAction("AccessDenied");
            }

            return View();

        }
        public IActionResult Index(int? page, string? category, string? search, string? ReturnUrl)
		{

            if (!search.IsNullOrEmpty() && category.IsNullOrEmpty())
            {
                var products = _products.Search(search).ToPagedList(page ?? 1, 20);
                ViewData["search"] = search;
                products.ToList().ForEach(a => a.key = _dataProtector.Protect(a.Id.ToString()));
                return View(products);
            }
            else if (!category.IsNullOrEmpty() && search.IsNullOrEmpty())
            {
                var products = _products.List().Where(a => a.category.Name == category).ToList().ToPagedList(page ?? 1, 20);
                ChecktheSession();
                products.ToList().ForEach(a => a.key = _dataProtector.Protect(a.Id.ToString()));
                return View(products);
            }
            else if (!search.IsNullOrEmpty() && !category.IsNullOrEmpty())
            {
                var listCate = _products.List().Where(a => a.category.Name == category);
                var res = listCate.Where(a => a.Name.Contains(search) || a.category.Name.Contains(search)).ToPagedList(page ?? 1, 20);
                ViewData["search"] = search;
                res.ToList().ForEach(a => a.key = _dataProtector.Protect(a.Id.ToString()));
                return View(res);
            }
            else
            {
                var products = _products.List().ToList().ToPagedList(page ?? 1, 20);
                ChecktheSession();
                products.ToList().ForEach(a => a.key = _dataProtector.Protect(a.Id.ToString()));
                return View(products);
            }
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