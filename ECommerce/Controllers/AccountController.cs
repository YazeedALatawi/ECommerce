using ECommerce.Models;
using ECommerce.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify;

namespace ECommerce.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<User> userManager;
		private readonly SignInManager<User> signInManager;
        private readonly IToastNotification _toastNotification;
		public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IToastNotification toastNotification)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
            _toastNotification = toastNotification;
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
                return Json(new { success = true });
            }
            else
			{
				return Json(new { success = false });
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
                        TempData["RegisterNotfi"] = "تم التسجيل بنجاح";
                        return RedirectToAction("index", "home");
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
