using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    public class AdminDashboardController : Controller
    {
        [Route("admin/dashboard/[action]/{id?}")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
