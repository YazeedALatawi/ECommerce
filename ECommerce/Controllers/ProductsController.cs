using ECommerce.Models;
using ECommerce.Models.Repositories;
using ECommerce.ViewModel;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ECommerce.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IOperations<Product> _products;
        private readonly IOperations<productOptions> _productOption;
        private readonly IOperations<options> _options;
        private readonly IToastNotification _ToastNotification;

        public ProductsController(IToastNotification toastNotification, IOperations<Product> product, IOperations<productOptions> productOption, IOperations<options> options)
        {
            _ToastNotification = toastNotification;
            _products = product;
            _productOption = productOption;
            _options = options;
        }
        public IActionResult Index()
        {
            return RedirectToAction("index","Home");
        }

        public IActionResult Details(int id)
        {
            var product = _products.Find(id);
            if(product != null)
            {
                product.ProductViewModel = ProductOptionReturn(id);
                return View(product);
            }

            return View("index");
        }



        ProductViewModel ProductOptionReturn(int productID)
        {
            var productOption = _productOption.List();
            var _model = new ProductViewModel
            {
                ProductId = productID,
                ExistingOptions = productOption
                    .Where(po => po.prdouctId == productID)
                    .Select(po => new MainOptionViewModel
                    {
                        MainOptionId = po.Id, // توفير معرف الخيار الرئيسي
                        MainOptionName = po.name,
                        SubOptions = po.Options?.Select(o => new SubOptionViewModel
                        {
                            SubOptionId = o.Id, // توفير معرف الخيار الفرعي
                            SubOptionName = o.Name,
                            SubOptionCount = o.count
                        }).ToList() ?? new List<SubOptionViewModel>()
                    }).ToList()
            };

            return _model;
        }
    }
}
