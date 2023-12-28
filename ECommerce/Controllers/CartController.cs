using ECommerce.Models;
using ECommerce.Models.Repositories;
using ECommerce.Models.Service;
using ECommerce.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NToastNotify;
using static System.Formats.Asn1.AsnWriter;

namespace ECommerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;
        private readonly IOperations<Product> _products;
        private readonly IOperations<Cart> _cart;
        private readonly UserManager<User> _userManager;
        private readonly IOperations<CartProducts> _cartProducts;
        private readonly IOperations<User> _userOperation;
        private readonly IOperations<Order> _order;
        private readonly IOperations<productOptions> _productOption;
        private readonly IOperations<options> _options;
        private readonly IOperations<CartPrdouctOptions> _cartProductsOptions;
        private readonly IToastNotification toastNotification;
        private readonly IDataProtector _dataProtector;

        public CartController(ShoppingCartService shoppingCartService, IOperations<Product> products, IOperations<Cart> cart, UserManager<User> userManager, IOperations<CartProducts> cartProducts, IOperations<User> userOperation, IOperations<Order> order, IOperations<productOptions> productOption, IOperations<options> options, IOperations<CartPrdouctOptions> cartProductsOptions, IToastNotification toastNotification, IDataProtectionProvider dataProtectionProvider)
        {
            _shoppingCartService = shoppingCartService;
            _products = products;
            _cart = cart;
            _userManager = userManager;
            _cartProducts = cartProducts;
            _userOperation = userOperation;
            _order = order;
            _productOption = productOption;
            _options = options;
            _cartProductsOptions = cartProductsOptions;
            this.toastNotification = toastNotification;
            _dataProtector = dataProtectionProvider.CreateProtector("ECommerce");
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var shoppingCart = _shoppingCartService.GetShoppingCart();
                double _totalPrice = 0;
                foreach (var x in shoppingCart.Products)
                {
                    x.allprice = x.price * x.Quantity;
                    _totalPrice += x.allprice;
                    x.key = _dataProtector.Protect(x.Id.ToString());

                    if(x.ProductViewModel == null)
                    {
                        x.ProductViewModel = new ProductViewModel();
                        x.ProductViewModel.ExistingOptions = ProductOptionReturnNoAuth(x.Id);
                    }
                    else
                    {
                        x.ProductViewModel.ExistingOptions = ProductOptionReturnNoAuth(x.Id);
                    }


                    
                }
                shoppingCart.totalPrice = _totalPrice;

                
                return View(shoppingCart);
            }
            else
            {
                var cart = _cart.findByIdUser(_userManager.GetUserId(User));
                if(cart == null)
                {
                    var shoppingCart = new ShoppingCart();
                    return View(shoppingCart);
                }
                else
                {
                    var allproduct = _cartProducts.findAllByCartId(cart.Id);
                    
                    var shoppingCart = new ShoppingCart();
                    foreach (var item in allproduct)
                    {
                        var theProduct = _products.Find(item.productId);
                        shoppingCart.Products.Add
                            (
                                new ProductCart
                                {
                                    Id = theProduct.Id,
                                    Name = theProduct.Name,
                                    image = theProduct.Image,
                                    Quantity = item.count,
                                    price = theProduct.Price,
                                    allprice = item.count * theProduct.Price,
                                    ProductViewModel = ProductOptionReturn(theProduct.Id, (List<CartProducts>)allproduct, cart),
                                    count = _products.Find(item.productId).Count,
                                    key = _dataProtector.Protect(theProduct.Id.ToString())
                                }
                            );

                    }

                    foreach(var item in shoppingCart.Products)
                    {
                        foreach(var option in item.ProductViewModel.ExistingOptions)
                        {
                            foreach(var subOption in option.SubOptions)
                            {
                                subOption.SubOptionCount = _options.Find(subOption.SubOptionId).count;
                            }
                        }
                    }

                    foreach(var item in shoppingCart.Products)
                    {
                        shoppingCart.totalPrice += item.allprice;

                    }


  
                    ChecktheSession();
                    return View(shoppingCart);
                }

            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int id, int? quantity, Product prod)
        {

            if (!User.Identity.IsAuthenticated)
            {
                var product = _products.Find(id);

                var shopCart = _shoppingCartService.GetShoppingCart();
                if (product != null)
                {
                    if (quantity == 0 || quantity == null)
                    {
                        quantity = 1;
                    }
                    ProductCart model = new ProductCart
                    {
                        Id = product.Id,
                        Name = product.Name,
                        price = product.Price,
                        image = product.Image,
                        Quantity = quantity.Value,
                        ProductViewModel = prod.ProductViewModel
                    };


                    var contain = shopCart.Products.SingleOrDefault(p => p.Id == id);
                    if (contain == null)
                    {

                        _shoppingCartService.AddToCart(model);
                        return RedirectToAction("Index", "Home");
                    }

                }

                return NoContent();
            }
            else
            {
                var product = _products.Find(id);
                if (product != null)
                {
                    var cart = _cart.findByIdUser(_userManager.GetUserId(User));
                    if(cart != null)
                    {
                        var CheckIsIn = _cartProducts.findAllByCartId(cart.Id);
                        var check = CheckIsIn.FirstOrDefault(x => x.productId == product.Id);
                        if(check != null)
                        {
                            return NoContent();
                        }

                        if(quantity == 0 || quantity == null)
                        {
                            quantity = 1;
                        }

                        var cartProduct = new CartProducts 
                        {
                            cartId = cart.Id,
                            count = quantity.Value,
                            productId = product.Id,
                        };

                        
                        _cartProducts.Add(cartProduct);
                        var theCartProducts = _cartProducts.List();


                        if (prod.ProductViewModel == null || prod.ProductViewModel.SelectedOptions == null || prod.ProductViewModel.SelectedOptions[0].SubOptionId == 0 || prod.ProductViewModel.SelectedOptions[0].OptionId == 0)
                        {

                        }
                        else
                        {
                            foreach (var item in prod.ProductViewModel.SelectedOptions)
                            {
                                _cartProductsOptions.Add
                                    (
                                        new CartPrdouctOptions
                                        {
                                            MainOptionID = item.OptionId,
                                            SubOptionID = item.SubOptionId,
                                            MainOptionName = _productOption.Find(item.OptionId).name,
                                            SubOptionName = _options.Find(item.SubOptionId).Name,
                                            CartProductsID = theCartProducts.FirstOrDefault(a => a.cartId == cart.Id && a.productId == product.Id).Id,
                                            ProductID = product.Id,
                                            cartID = cart.Id

                                        }
                                    );

                            }
                        }



                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {

                        var newCart = new Cart
                        {
                            userId = _userManager.GetUserId(User),
                            isDelete = false,

                        };

                        _cart.Add(newCart);
                        var theCart = _cart.findByIdUser(_userManager.GetUserId(User));

                        if (quantity == 0 || quantity == null)
                        {
                            quantity = 1;
                        }

                        var cartProduct = new CartProducts
                        {
                            cartId = theCart.Id,
                            count = quantity.Value,
                            productId = product.Id,
                        };

                        _cartProducts.Add(cartProduct);
                        var theCartProducts = _cartProducts.List();
                        if (prod.ProductViewModel == null || prod.ProductViewModel.SelectedOptions == null || prod.ProductViewModel.SelectedOptions[0].SubOptionId == 0 || prod.ProductViewModel.SelectedOptions[0].OptionId == 0)
                        {

                        }
                        else
                        {
                            foreach (var item in prod.ProductViewModel.SelectedOptions)
                            {
                                _cartProductsOptions.Add
                                    (
                                        new CartPrdouctOptions
                                        {
                                            MainOptionID = item.OptionId,
                                            SubOptionID = item.SubOptionId,
                                            MainOptionName = _productOption.Find(item.OptionId).name,
                                            SubOptionName = _options.Find(item.SubOptionId).Name,
                                            CartProductsID = theCartProducts.FirstOrDefault(a => a.cartId == theCart.Id && a.productId == product.Id).Id,
                                            ProductID = product.Id,
                                            cartID = theCart.Id

                                        }
                                    );

                            }
                        }



                        return RedirectToAction("Index", "Home");

                    }
                }

                return NoContent();
            }
            

        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                _shoppingCartService.RemoveProduct(productId);
                return RedirectToAction("Index");
            }
            else
            {
                var cartID = _cart.findByIdUser(_userManager.GetUserId(User));
                if (cartID == null)
                {
                    return View("index");
                }
                else
                {
                    var allProducts = _cartProducts.findAllByCartId(cartID.Id);
                    var theProduct = allProducts.FirstOrDefault(x => x.productId == productId);
                    _cartProducts.delete(theProduct.Id);

                    var productsAfterDelete = _cartProducts.findAllByCartId(cartID.Id);
                    if(productsAfterDelete.Count == 0)
                    {
                        cartID.isDelete = true;
                        _cart.update(cartID);
                    }

                    return RedirectToAction("Index");
                }
            }

        }


        [HttpPost]

        public IActionResult incrementQuantity(int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                quantity++;
                _shoppingCartService.UpdateQuantity(productId, quantity);
                return RedirectToAction("Index");
            }
            else
            {
                var cartID = _cart.findByIdUser(_userManager.GetUserId(User));
                var allProducts = _cartProducts.findAllByCartId(cartID.Id);
                var theProduct = allProducts.FirstOrDefault(x => x.productId == productId);
                quantity++;
                theProduct.count = quantity;
                _cartProducts.update(theProduct);
                //return RedirectToAction("Index");
                return Json(theProduct.count);
            }

        }

        [HttpPost]

        public IActionResult decrementQuantity(int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                quantity--;
                _shoppingCartService.UpdateQuantity(productId, quantity);
                return RedirectToAction("Index");
            }
            else
            {
                var cartID = _cart.findByIdUser(_userManager.GetUserId(User));
                var allProducts = _cartProducts.findAllByCartId(cartID.Id);
                var theProduct = allProducts.FirstOrDefault(x => x.productId == productId);
                quantity--;
                theProduct.count = quantity;
                _cartProducts.update(theProduct);
                //return RedirectToAction("Index");
                return Json(theProduct.count);

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public IActionResult _Checkout(ShoppingCart shopping)
        {

            if (User.Identity.IsAuthenticated)
            {

                var theCart = _cart.findByIdUser(_userManager.GetUserId(User));
                if(theCart != null && theCart.isDelete != true)
                {
                    var theAllproducts = _cartProducts.findAllByCartId(theCart.Id);
                    double theTotalPrice = 0;
                    if(theAllproducts.Count > 0)
                    {
                        foreach(var prod in shopping.Products)
                        {
                            if(prod.ProductViewModel != null)
                            {
                                foreach (var options in prod.ProductViewModel.SelectedOptions)
                                {
                                    if (options.OptionId != 0 && options.SubOptionId != 0)
                                    {
                                        var cartProductOption = _cartProductsOptions.List().FirstOrDefault(a => a.cartID == theCart.Id && a.ProductID == prod.Id && a.MainOptionID == options.OptionId && a.SubOptionID == options.SubOptionId);
                                        if (cartProductOption == null)
                                        {
                                            var checkIsInTable = _cartProductsOptions.List().FirstOrDefault(a => a.cartID == theCart.Id && a.ProductID == prod.Id && a.MainOptionID == options.OptionId);
                                            if (checkIsInTable != null)
                                            {
                                                var theProductInFinalCart = theAllproducts.FirstOrDefault(a => a.cartId == theCart.Id && a.productId == prod.Id);
                                                var theOptionCountInStorage = _options.List().FirstOrDefault(a => a.Id == options.SubOptionId);
                                                if(theProductInFinalCart.count != 0 && theProductInFinalCart.count <= theOptionCountInStorage.count && theOptionCountInStorage.count != 0)
                                                {
                                                    var subOptionsName = _options.Find(options.SubOptionId).Name;
                                                    checkIsInTable.SubOptionID = options.SubOptionId;
                                                    checkIsInTable.SubOptionName = subOptionsName;
                                                    _cartProductsOptions.update(checkIsInTable);
                                                }
                                                else
                                                {
                                                    toastNotification.AddErrorToastMessage($"نفذت كمية {prod.Name} الكمية المتاحه ({theOptionCountInStorage.count.ToString()})");
                                                    return RedirectToAction("index");
                                                }

                                            }
                                            else
                                            {

                                                var theProductInFinalCart = theAllproducts.FirstOrDefault(a => a.cartId == theCart.Id && a.productId == prod.Id);
                                                var theOptionCountInStorage = _options.List().FirstOrDefault(a => a.Id == options.SubOptionId);
                                                if (theProductInFinalCart.count != 0 && theProductInFinalCart.count <= theOptionCountInStorage.count && theOptionCountInStorage.count != 0)
                                                {
                                                    var cartProductsID = _cartProducts.List().FirstOrDefault(a => a.productId == prod.Id && a.cartId == theCart.Id).Id;
                                                    var mainOptionName = _productOption.Find(options.OptionId).name;
                                                    var subOptionsName = _options.Find(options.SubOptionId).Name;
                                                    if (cartProductsID != null && mainOptionName != null && subOptionsName != null)
                                                    {
                                                        var newCartProductsOptions = new CartPrdouctOptions
                                                        {
                                                            SubOptionID = options.SubOptionId,
                                                            MainOptionID = options.OptionId,
                                                            cartID = theCart.Id,
                                                            ProductID = prod.Id,
                                                            SubOptionName = subOptionsName,
                                                            MainOptionName = mainOptionName,
                                                            CartProductsID = cartProductsID,
                                                        };

                                                        _cartProductsOptions.Add(newCartProductsOptions);
                                                    }
                                                    else
                                                    {
                                                        toastNotification.AddErrorToastMessage("خطأ");
                                                        return RedirectToAction("Index");
                                                    }
                                                }
                                                else
                                                {
                                                    toastNotification.AddErrorToastMessage($"نفذت كمية {prod.Name} الكمية المتاحه ({theOptionCountInStorage.count.ToString()})");
                                                    return RedirectToAction("index");
                                                }



                                            }



                                        }
                                        else
                                        {
                                            var theProductInFinalCart = theAllproducts.FirstOrDefault(a => a.cartId == theCart.Id && a.productId == prod.Id);
                                            var theOptionCountInStorage = _options.List().FirstOrDefault(a => a.Id == options.SubOptionId);
                                            if (theProductInFinalCart.count != 0 && theProductInFinalCart.count <= theOptionCountInStorage.count && theOptionCountInStorage.count != 0)
                                            {

                                            }
                                            else
                                            {
                                                toastNotification.AddErrorToastMessage($"نفذت كمية {prod.Name} الكمية المتاحه ( {theOptionCountInStorage.count.ToString()} )");
                                                return RedirectToAction("index");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        toastNotification.AddErrorToastMessage("الرجاء اختيار خيارات المنتجات");
                                        return RedirectToAction("Index");
                                    }
                                }
                            }
                            else
                            {
                                var check = _productOption.List().FirstOrDefault(a => a.prdouctId == prod.Id);
                                if(check != null)
                                {
                                    toastNotification.AddErrorToastMessage("الرجاء اختيار خيارات المنتج");
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    var product = _products.Find(prod.Id);
                                    var theProductInFinalCart = theAllproducts.FirstOrDefault(a => a.cartId == theCart.Id && a.productId == prod.Id);
                                    if(product.Count != 0 && theProductInFinalCart.count <= product.Count && theProductInFinalCart.count != 0)
                                    {

                                    }
                                    else
                                    {
                                        toastNotification.AddErrorToastMessage($"نفذت كمية {product.Name} الكمية المتاحة ({product.Count})");
                                        return RedirectToAction("index");
                                    }
                                }
                            }
                            
                        }
                        
                        foreach (var item in theAllproducts)
                        {
                            var product = _products.Find(item.productId);
                            theTotalPrice = theTotalPrice + product.Price * item.count;
                        }
                        var TheUser = _userOperation.FindString(_userManager.GetUserId(User));
                        CheckOutViewModel model = new CheckOutViewModel
                        {
                            CartId = theCart.Id,
                            firstName = TheUser.FirstName,
                            LastName = TheUser.LastName,
                            City = TheUser.City,
                            District = TheUser.District,
                            shippingCost = 0,
                            Street = TheUser.Street,
                            totalPrice = theTotalPrice
                        };
                        model.totalPrice = model.totalPrice + model.shippingCost;

                        theCart.totalPrice = model.totalPrice;
                        _cart.update(theCart);

                        return View("Checkout", model);
                    }
                    else
                    {
                        toastNotification.AddErrorToastMessage("السله خاليه");
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    toastNotification.AddErrorToastMessage("السله خاليه");
                    return RedirectToAction("index");
                }

               
            }
            else
            {
                toastNotification.AddErrorToastMessage("الرجاء تسجيل الدخول و التسوق");
                return RedirectToAction("index");
            }

        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(CheckOutViewModel view)
        {

            if (User.Identity.IsAuthenticated)
            {
                var TheCart = _cart.findByIdUser(_userManager.GetUserId(User));
                if (TheCart == null)
                {
                    return RedirectToAction("index", "home");
                }

                if(view.totalPrice != TheCart.totalPrice)
                {
                    return RedirectToAction("index", "home");
                }

                



                var newOrder = new Order
                {
                    cartId = TheCart.Id,
                    orderState = "جديد",
                    OrderDate = DateTime.Now,
                    totalPrice = TheCart.totalPrice,
                    shipping = ""
                };

                var user = _userOperation.findByIdUser(TheCart.userId);
                user.Street = view.Street; user.City = view.City; user.District = view.District;
                _userOperation.update(user);


                var theAllproducts = _cartProducts.findAllByCartId(TheCart.Id);
                if (theAllproducts == null)
                {
                    toastNotification.AddErrorToastMessage("لاتوجد منتجات بالسله");
                    return RedirectToAction("index");
                }
                else
                {
                    var itemInCart = new List<CartPrdouctOptions>();
                    foreach (var item in theAllproducts)
                    {
                        item.product = _products.Find(item.productId);
                        item.options = _cartProductsOptions.List().Where(a => a.cartID == TheCart.Id && a.ProductID == item.product.Id).ToList();
                        if (item.product != null)
                        {
                            if (item.options != null)
                            {
                                foreach (var option in item.options)
                                {
                                    var theProductInFinalCart = theAllproducts.FirstOrDefault(a => a.cartId == TheCart.Id && a.productId == item.productId);
                                    var theOptionCountInStorage = _options.List().FirstOrDefault(a => a.Id == option.SubOptionID);
                                    if (theProductInFinalCart.count != 0 && theProductInFinalCart.count <= theOptionCountInStorage.count && theOptionCountInStorage.count != 0)
                                    {
                                        theOptionCountInStorage.count = theOptionCountInStorage.count - theProductInFinalCart.count;
                                        _options.update(theOptionCountInStorage);
                                    }
                                    else
                                    {
                                        toastNotification.AddErrorToastMessage($"نفذت كمية {item.product.Name} الكمية المتاحة ({theOptionCountInStorage.count.ToString()})");
                                        return RedirectToAction("index");
                                    }
                                }
                            }
                            else
                            {
                                var product = _products.Find(item.product.Id);
                                var theProductInFinalCart = theAllproducts.FirstOrDefault(a => a.cartId == TheCart.Id && a.productId == item.product.Id);
                                if (product.Count != 0 && theProductInFinalCart.count <= product.Count && theProductInFinalCart.count != 0)
                                {
                                    product.Count = -theProductInFinalCart.count;
                                    _products.update(product);
                                }
                                else
                                {
                                    toastNotification.AddErrorToastMessage($"نفذت كمية {product.Name} الكمية المتاحة ({product.Count})");
                                    return RedirectToAction("index");
                                }
                            }
                        }
                        else
                        {
                            return RedirectToAction("index");
                        }



                    }



                }


                _order.Add(newOrder);
                TheCart.isDelete = true;
                _cart.update(TheCart);

                return RedirectToAction("index", "Order");
            }
            else
            {
                return RedirectToAction("index", "home");
            }

        }

        public void ChecktheSession()
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

        ProductViewModel ProductOptionReturn(int productID, List<CartProducts> allProducts, Cart cart)
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
                    }).ToList(),

            };

            var TheCart = _cart.findByIdUser(_userManager.GetUserId(User));
            var theCartproducts = _cartProducts.List().Where(a => a.cartId == TheCart.Id).ToList();
            _model.SelectedOptions = _cartProductsOptions.List().Where(a => a.ProductID == productID && a.cartID == cart.Id).Select(po => new ProductOptionViewModel { OptionId = po.MainOptionID, SubOptionId = po.SubOptionID }).ToList();

            //foreach (var mainOption in _model.ExistingOptions)
            //{
            //    mainOption.SubOptions.Insert(0, new SubOptionViewModel
            //    {
            //        SubOptionId = -1,
            //        SubOptionName = "اختار " + mainOption.MainOptionName,
            //        MainOptionId = mainOption.MainOptionId,
            //        SubOptionCount = -1
            //    });
            //}
            return _model;
        }

        List<MainOptionViewModel> ProductOptionReturnNoAuth(int productID)
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
                    }).ToList(),

            };

            //var TheCart = _cart.findByIdUser(_userManager.GetUserId(User));
            //var theCartproducts = _cartProducts.List().Where(a => a.cartId == TheCart.Id).ToList();
            //_model.SelectedOptions = _cartProductsOptions.List().Where(a => a.ProductID == productID && a.cartID == cart.Id).Select(po => new ProductOptionViewModel { OptionId = po.MainOptionID, SubOptionId = po.SubOptionID }).ToList();

            //foreach (var mainOption in _model.ExistingOptions)
            //{
            //    mainOption.SubOptions.Insert(0, new SubOptionViewModel
            //    {
            //        SubOptionId = -1,
            //        SubOptionName = "اختار " + mainOption.MainOptionName,
            //        MainOptionId = mainOption.MainOptionId,
            //        SubOptionCount = -1
            //    });
            //}
            return _model.ExistingOptions;
        }
    }
    
}
