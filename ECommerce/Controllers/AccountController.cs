using ECommerce.Models;
using ECommerce.Models.Repositories;
using ECommerce.Models.Service;
using ECommerce.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ECommerce.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IToastNotification _toastNotification;
        private readonly IOperations<User> _userOperation;
        private readonly IOperations<Product> _products;
        private readonly IOperations<Cart> _carts;
        private readonly IOperations<CartProducts> _cartProducts;
        private readonly ShoppingCartService _shoppingCartService;
        private readonly RoleManager<IdentityRole> _roleManger;
        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManger, SignInManager<User> signInManager, IToastNotification toastNotification, ShoppingCartService shoppingCartService, IOperations<Product> products, IOperations<Cart> cart, IOperations<CartProducts> cartProducts, IOperations<User> userOperation)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _toastNotification = toastNotification;
            _userOperation = userOperation;
            _products = products;
            _carts = cart;
            _cartProducts = cartProducts;
            _shoppingCartService = shoppingCartService;
            _roleManger = roleManger;
        }

        [Authorize]


        public IActionResult index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = _userOperation.findByIdUser(userManager.GetUserId(User));
                var model = new UserViewModel
                {
                    id = user.Id,
                    Street = user.Street,
                    City = user.City,
                    District = user.District,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber
                };


                return View(model);
            }

            return RedirectToAction("index", "home");
        }


        [Authorize]
        public async Task<IActionResult> update(UserViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var userID = userManager.GetUserId(User);
                    var user = await userManager.FindByIdAsync(userID);
                    if (await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        _toastNotification.AddWarningToastMessage("لا يسمح بتغير بيانات هذا المستخدم");
                        return RedirectToAction("Index");
                    }
                    user.PhoneNumber = model.PhoneNumber;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.City = model.City;
                    user.District = model.District;
                    user.Email = model.Email;
                    user.Street = model.Street;
                    user.UserName = model.Email;

                    var res = await userManager.UpdateAsync(user);

                    if (res.Succeeded)
                    {
                        _toastNotification.AddSuccessToastMessage("تم تعديل البيانات");
                        return RedirectToAction("index");
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("خطأ");
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("خطا في ادخال البيانات");
                    return RedirectToAction("index");
                }

            }
            else
            {
                return RedirectToAction("index", "home");
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> changePassword(UserViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid && model.RePassword == model.ConfirmPassword)
                {
                    var userID = userManager.GetUserId(User);
                    var user = await userManager.FindByIdAsync(userID);

                    if (await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        _toastNotification.AddWarningToastMessage("لا يسمح بتغير بيانات هذا المستخدم");
                        return RedirectToAction("Index");
                    }

                    IdentityResult check = await signInManager.UserManager.ChangePasswordAsync(user, model.Password, model.RePassword);
                    if (check.Succeeded)
                    {
                        _toastNotification.AddSuccessToastMessage("تم تغير كلمة السر بنجاح");
                        return RedirectToAction("index");
                    }
                    else
                    {
                        _toastNotification.AddErrorToastMessage("كلمة السر الحالية خطأ");
                        return RedirectToAction("index");
                    }
                }
                else
                {
                    _toastNotification.AddErrorToastMessage("خطأ في ادخال البيانات");
                    return RedirectToAction("index");
                }


            }
            else
            {
                return RedirectToAction("index", "home");
            }

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(ViewModelLogin user)
        {
            var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);
            if (result.Succeeded)
            {
                if (user.Email == "Admin@gmail.com")
                {
                    await AddAdmin();
                }

                return Json(new { success = true });
                // Admin@gmail.com, Admin123
                // customer@gmail.com Customer123
            }
            else
            {
                return Json(new { success = false });
            }

        }

        async Task AddAdmin()
        {
            var roleExists = await _roleManger.RoleExistsAsync("Admin");
            if (!roleExists)
            {
                await _roleManger.CreateAsync(new IdentityRole("Admin"));
            }

            var admin = await userManager.FindByEmailAsync("Admin@gmail.com");
            if (admin != null)
            {
                var isInRole = await userManager.IsInRoleAsync(admin, "Admin");
                if (!isInRole)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                _toastNotification.AddWarningToastMessage("سجل خروج حتا تتمكن من الوصول الى صفحة تسجيل جديد");

                return RedirectToAction("index", "Home");
            }
            var citys = new ViewModelRegister();
            return View(citys);
        }


        [HttpPost]
        public async Task<IActionResult> Register(ViewModelRegister user)
        {
            if (ModelState.IsValid)
            {
                var newuser = new User
                {
                    UserName = user.Email,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    City = user.City,
                    Street = user.Street,
                    District = user.District,
                };

                var USER = userManager.FindByEmailAsync(newuser.Email);
                if (USER.Result == null)
                {
                    var result = await userManager.CreateAsync(newuser, user.Password);

                    if (result.Succeeded)
                    {

                        var productsSession = _shoppingCartService.GetShoppingCart();
                        if (productsSession == null || productsSession.Products.Count == 0)
                        {
                            TempData["RegisterNotfi"] = "تم التسجيل بنجاح";
                            return RedirectToAction("index", "home");
                        }
                        else
                        {
                            var theCart = _carts.findByIdUser(userManager.GetUserId(User));
                            if (theCart == null)
                            {
                                var newCart = new Cart
                                {
                                    userId = userManager.GetUserId(User)
                                };

                                _carts.Add(newCart);
                                var Thecart = _carts.findByIdUser(userManager.GetUserId(User));
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
                                TempData["RegisterNotfi"] = "تم التسجيل بنجاح";
                                return RedirectToAction("index", "home");
                            }
                            else
                            {
                                var Thecart = _carts.findByIdUser(userManager.GetUserId(User));
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
                                TempData["RegisterNotfi"] = "تم التسجيل بنجاح";
                                return RedirectToAction("index", "home");
                            }
                        }


                    }
                    else
                    {
                        TempData["errorRegister"] = result.Errors.ToArray();
                    }
                }
                else
                {
                    TempData["errorRegister"] = "هذا الايميل مستخدم بالفعل";
                }


            }


            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }


        string city(int num)
        {
            switch (num)
            {
                case 1:
                    return "الرياض";
                case 2:
                    return "الدمام";
                case 3:
                    return "بريدة";
                case 4:
                    return "حائل";
                case 5:
                    return "تبوك";
                case 6:
                    return "الخرج";
                case 7:
                    return "خميس مشيط";
                case 8:
                    return "الخفجي";
                case 9:
                    return "الخبر";
                case 10:
                    return "المدينة المنورة";
                case 11:
                    return "مكة";
                case 12:
                    return "جدة";
                case 13:
                    return "ابها";
                case 14:
                    return "جازان";
                case 15:
                    return "ينبع";
                case 16:
                    return "ضباء";
                default:
                    return "";
            }
        }

    }
}
