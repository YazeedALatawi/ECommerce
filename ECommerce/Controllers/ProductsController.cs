using ECommerce.Models;
using ECommerce.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ECommerce.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IOperations<Product> _products;
        private readonly IToastNotification _ToastNotification;

        public ProductsController(IToastNotification toastNotification, IOperations<Product> product)
        {
            _ToastNotification = toastNotification;
            _products = product;
        }
        public IActionResult Index()
        {
            return RedirectToAction("index","Home");
        }

        public IActionResult Details(int id)
        {
            var product = _products.Find(id);
            return View(product);
        }
    }
}
