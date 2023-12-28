using ECommerce.Models;
using ECommerce.Models.Repositories;
using ECommerce.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("admin/dashboard/[action]/{id?}")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        private readonly IOperations<Order> _orders;
		private readonly IOperations<Cart> _carts;
		private readonly IOperations<User> _user;
		public AdminDashboardController(IOperations<Order> orders, IOperations<Cart> carts, IOperations<User> users)
        {
            _carts= carts;
            _user= users;
            _orders= orders;
        }
        public IActionResult Index()
        {
            double profits = 0;
            var carts = _carts.List();
            foreach(var i in carts)
            {

                if (_orders.List().Any(a => a.cartId == i.Id))
                {
					profits = profits + i.totalPrice;
				}

            }

            var model = new DashBoardViewModel
            {
                numOrders = _orders.List().Count(),
                numOrdersUndelivered = _orders.List().Where(a => a.orderState != "تم الشحن" && a.orderState != "ملغي" && a.orderState != "تم الاستلام من العميل").Count(),
                NumUser = _user.List().Count(),
                Profits = profits
            };

            return View(model);

        }
    }
}
