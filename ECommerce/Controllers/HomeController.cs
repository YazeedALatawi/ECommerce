using ECommerce.Models;
using ECommerce.Models.Repositories;
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

		public HomeController(ILogger<HomeController> logger, IToastNotification toastNotification, IOperations<Product> product)
		{
			_logger = logger;
			_ToastNotification = toastNotification;
			_products= product;
		}

		public IActionResult Index()
		{
			var products = _products.List().ToList();
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

	}
}