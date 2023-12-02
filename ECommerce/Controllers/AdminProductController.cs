using ECommerce.Models;
using ECommerce.Models.Repositories;
using ECommerce.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Differencing;
using NToastNotify;
using static NuGet.Packaging.PackagingConstants;

namespace ECommerce.Controllers
{
    [Route("admin/Product/[action]/{id?}")]
    public class AdminProductController : Controller
    {
        private readonly IOperations<Product> _products;
        private readonly IOperations<Category> _category;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IToastNotification toastNotification;

        public AdminProductController(IOperations<Product> products, IOperations<Category> category, IWebHostEnvironment webHostEnvironment, IToastNotification toastNotification)
        {
            this._products = products;
            this._category = category;
            this.webHostEnvironment = webHostEnvironment;
            this.toastNotification = toastNotification;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var listProducts = _products.List();
            return View(listProducts);
        }

        public IActionResult Create()
        {
            var viewmodel = new ViewModelProduct()
            {
                categoryies = _category.List().ToList()
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ViewModelProduct product)
        {
            if (ModelState.IsValid)
            {
                if(product.image != null)
                {
                    string folder = "img/Product_img/";
                    folder = folder + Guid.NewGuid().ToString() + "_" + product.image.FileName;
                    string allPath = folder.ToString();
                    string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                    FileStream fs = new FileStream(serverFolder, FileMode.Create);
                    product.image.CopyToAsync(fs);
                    fs.Close();

                    var TheCategory = _category.Find(product.categoryId);
                    Product entity = new Product
                    {
                        Name = product.Name,
                        Count = product.Count,
                        Description = product.Description,
                        Discount = product.Discount,
                        Image = allPath,
                        MadeIn = product.MadeIn,
                        Price = product.Price,
                        size = product.size,
                        category = TheCategory,
                    };
                    entity.category.Id = 0;
                    _products.Add(entity);
                    toastNotification.AddSuccessToastMessage("تم اضافة المنتج");
                    return RedirectToAction("index", "Product");

                }

                product.categoryies = _category.List().ToList();
                return View(product);
            }

            return View(product);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var model = _products.Find(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _products.delete(id);
            toastNotification.AddSuccessToastMessage("تم الحذف المنتج");
            return RedirectToAction("index", "Product");
        }

        public IActionResult Edit(int id)
        {
            var model = _products.Find(id);
            ViewModelEditProduct prodModel = new ViewModelEditProduct
            {
                id = model.Id,
                Name = model.Name,
                Count = model.Count,
                Description = model.Description,
                Discount = model.Discount,
                size = model.size,
                categoryId = model.category.Id,
                imageUrl = model.Image,
                MadeIn = model.MadeIn,
                Price = model.Price,
                categoryies = _category.List().ToList(),
            };

            return View(prodModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ViewModelEditProduct model, int id)
        {

            bool check = false;
            string allNewImage = "";
            if (model.image != null)
            {
                string folder = "img/Product_img/";
                string oldfileName = _products.Find(model.id).Image.Remove(0, 16);
                string serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                string fullOldPathFile = Path.Combine(serverFolder, oldfileName);
                System.IO.File.Delete(fullOldPathFile);

                folder = folder + Guid.NewGuid().ToString() + "_" + model.image.FileName;
                allNewImage = folder.ToString();
                serverFolder = Path.Combine(webHostEnvironment.WebRootPath, folder);
                FileStream fs = new FileStream(serverFolder, FileMode.Create);
                model.image.CopyToAsync(fs);
                fs.Close();
                check = true;
            }

            Product productModel = new Product
            {
                category = _category.Find(model.categoryId),
                size = model.size,
                Count = model.Count,
                Description = model.Description,
                Discount = model.Discount,
                MadeIn = model.MadeIn,
                Name = model.Name,
                Price = model.Price,
                Id = id,
                Image = check != true ? _products.Find(model.id).Image : allNewImage
            };
                
            _products.update(productModel);

            return RedirectToAction("index","Product");
        }
    }
}
