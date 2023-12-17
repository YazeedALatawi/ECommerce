using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("admin/dashboard/[action]/{id?}")]
    public class AdminDashboardController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
