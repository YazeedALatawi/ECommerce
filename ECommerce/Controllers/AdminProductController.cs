using System.Configuration;
using System.Text.Encodings.Web;
using ECommerce.Models;
using ECommerce.Models.Repositories;
using ECommerce.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
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
		private readonly IOperations<productOptions> _productOption;
        private readonly IOperations<options> _options;

        public AdminProductController(IOperations<Product> products, IOperations<Category> category, IWebHostEnvironment webHostEnvironment, IToastNotification toastNotification, IOperations<productOptions> productOption, IOperations<options> options)
        {
            this._products = products;
            this._category = category;
            this.webHostEnvironment = webHostEnvironment;
            this.toastNotification = toastNotification;
			_productOption = productOption;
            _options = options;
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
                        CategoryId = product.categoryId,
                    };

                    double afterDiscount = 0;
                    if(entity.Discount > 0 && entity.Discount != null)
                    {
                        afterDiscount = entity.Price - (entity.Price * (double)entity.Discount / 100);
                        entity.Price = afterDiscount;
                    }
                    _products.Add(entity);
                    toastNotification.AddSuccessToastMessage("تم اضافة المنتج");
                    return RedirectToAction("index", "AdminProduct");

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
            return RedirectToAction("index", "AdminProduct");
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

            return RedirectToAction("index","AdminProduct");
        }


        [HttpGet]
        public IActionResult Options(int id)
        {
            var model = _products.Find(id);
            var productOption = _productOption.List();

            var _model = new ProductViewModel
            {
                ProductId = id,
                ExistingOptions = productOption
                    .Where(po => po.prdouctId == id)
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


            return View(_model);
        }


        [HttpPost]
        public IActionResult AddOptions(ProductViewModel viewModel)
        {
            var product = _products.Find(viewModel.ProductId);

            if (product != null)
            {
                if (product.ProductViewModel == null)
                {
                    product.ProductViewModel = new ProductViewModel();
                }

                product.ProductViewModel.MainOptions = viewModel.MainOptions;

                if (product.ProductViewModel.MainOptions != null)
                {
                    foreach (var mainOption in product.ProductViewModel.MainOptions)
                    {
                        var productOption = new productOptions
                        {
                            name = mainOption.MainOptionName,
                            prdouctId = viewModel.ProductId,
                            Options = new List<options>()
                        };

                        if(mainOption.SubOptions == null)
                        {
                            toastNotification.AddErrorToastMessage("الرجاء اضافة اسم الخيار الرئيسي واضافة الخيارات الفرعيه له ثم الحفظ");
                            return RedirectToAction("Options", new { id = viewModel.ProductId });
                        }

                        foreach (var subOption in mainOption.SubOptions)
                        {
                            productOption.Options.Add(new options
                            {
                                Name = subOption.SubOptionName,
                                count = subOption.SubOptionCount
                            });
                        }

                        _productOption.Add(productOption);
                        toastNotification.AddSuccessToastMessage("تم الاضافة بنجاح");
                        return RedirectToAction("Options", new {id = viewModel.ProductId});

                    }

                    // يمكنك حفظ التغييرات في قاعدة البيانات هنا
                }
            }

            // يمكنك الوصول إلى viewModel وتحديث قاعدة البيانات أو القيام بالمعالجة اللازمة
            // ثم إعادة توجيه المستخدم أو عرض عرض آخر حسب حاجتك
            toastNotification.AddErrorToastMessage("الرجاء اضافة اسم الخيار الرئيسي واضافة الخيارات الفرعيه له ثم الحفظ");
            return RedirectToAction("Options", new { id = viewModel.ProductId });
        }


        [HttpPost]
        public IActionResult DeleteSubOption(int productId, int mainOptionId, int subOptionId)
        {
            // ابحث عن الخيار الرئيسي والفرعي الذي تم حذفه
            var optionToDelete = _productOption.List()
                .FirstOrDefault(po => po.prdouctId == productId && po.Id == mainOptionId);

            if (optionToDelete != null)
            {
                // ابحث عن الخيار الفرعي داخل الخيار الرئيسي
                var subOptionToDelete = optionToDelete.Options?.FirstOrDefault(o => o.Id == subOptionId);

                if (subOptionToDelete != null)
                {
                    // حذف الخيار الفرعي
                    optionToDelete.Options.Remove(subOptionToDelete);
                    _options.delete(subOptionToDelete.Id);

                    // حفظ التغييرات في قاعدة البيانات
                }
            }

            // بعد الحذف، قم بإعادة توجيه المستخدم إلى صفحة الإضافة
            return RedirectToAction("Options", new { id = productId });
        }

        [HttpPost]
        public IActionResult DeleteMainOption(int productId, int mainOptionId)
        {
            // ابحث عن الخيار الرئيسي والفرعي الذي تم حذفه
            var option = _productOption.List();
            var optionToDelete = option.FirstOrDefault(x => x.Id == mainOptionId);

            if (optionToDelete != null)
            {

                foreach (var item in optionToDelete.Options)
                {
                    _options.delete(item.Id);
                }


                _productOption.delete(optionToDelete.Id);
            }

            // بعد الحذف، قم بإعادة توجيه المستخدم إلى صفحة الإضافة
            return RedirectToAction("Options", new { id = productId });
        }

        [HttpPost]
        public IActionResult AddSubOption(int productId, int mainOptionId, string subOptionName, int subOptionCount)
        {
            var mainOption = _productOption.Find(mainOptionId);
            if (mainOption != null)
            {
                var option = new options
                {
                    Name = subOptionName,
                    count = subOptionCount,
                    productOptionsId = mainOption.Id
                };

                _options.Add(option);
                toastNotification.AddSuccessToastMessage("تم الاضافة بنجاح");
                return RedirectToAction("Options", new { id = productId });
            }

            return RedirectToAction("Options", new { id = productId });
        }




    }
}
