using ECommerce.Models.Repositories;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using ECommerce.ViewModel;
using X.PagedList;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ECommerce.Controllers
{
    [Route("admin/Order/[action]/{id?}")]
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private readonly IOperations<Order> _order;
        private readonly IOperations<Cart> _cart;
        private readonly IOperations<CartProducts> _cartProducts;
        private readonly IOperations<Product> _product;
        private readonly IOperations<Shipping> _shipping;
        private readonly IOperations<User> _userOperations;

        public AdminOrderController(IOperations<Order> order, IOperations<Cart> cart, IOperations<CartProducts> cartProducts, IOperations<Product> product, IOperations<Shipping> shipping, IOperations<User> userOperations)
        {
            _order = order;
            _cart = cart;
            _cartProducts = cartProducts;
            _product = product;
            _shipping = shipping;
            _userOperations = userOperations;
        }

        IPagedList<Order> sort(string colName, int? page)
        {
            if (colName == "num")
            {
                var res = _order.List().OrderByDescending(a => a.Id).ToPagedList(page ?? 1, 30);
                return res;
            }
            else if (colName == "pri")
            {
                var res = _order.List().OrderByDescending(a => a.totalPrice).ToPagedList(page ?? 1, 30);
                return res;
            }
            else if (colName == "state")
            {
                var res = _order.List().OrderBy(a => a.orderState).ToPagedList(page ?? 1, 30);
                return res;
            }
            else if (colName == "ship")
            {
                var res = _order.List().OrderBy(a => a.shipping).ToPagedList(page ?? 1, 30);
                return res;
            }
            else if (colName == "date")
            {
                var res = _order.List().OrderByDescending(a => a.OrderDate).ToPagedList(page ?? 1, 30);
                return res;
            }
            else
            {
                return null;
            }
        }
        IPagedList<Order> Search(string search, int? page)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return null;
            }
            else
            {
                var res = _order.Search(search).ToPagedList(page ?? 1, 10);
                return res;
            }

        }
        public IActionResult Index(int? page, string? colName, string search)
        {
            if (colName != null && search == null)
            {
                var listSort = sort(colName, page);
                if (listSort != null)
                {
                    return View(listSort);
                }
            }

            if (search != null)
            {
                var listSort = Search(search, page);
                if (listSort != null)
                {
                    return View(listSort);
                }
            }

            var orders = _order.List().OrderByDescending(a => a.Id).ToPagedList(page ?? 1, 10);
            return View(orders);
        }
        [HttpGet]
        public IActionResult Details(int Id) 
        {

            var Order = _order.Find(Id);
            var Cart = _cart.Find(Order.cartId);
            var allProducts = _cartProducts.findAllByCartId(Cart.Id);
            var userId = _cart.Find(Cart.Id).userId;
            var theUser = _userOperations.findByIdUser(userId);
            foreach (var item in allProducts)
            {
                item.product = _product.Find(item.productId);
            }
            var model = new orderAdminViewModel
            {
                Id = Order.Id,
                cartProduct = (List<CartProducts>)allProducts,
                orderState = Order.orderState,
                shipping = Order.shipping,
                shippingList = (List<Shipping>)_shipping.List(),
                User = theUser
            };
           
            return View(model);
        }

        [HttpPost]
        public IActionResult Details(orderAdminViewModel model)
        {
            var order = _order.Find(model.Id);
            order.shipping = model.shipping;
            order.orderState = model.orderState;
            _order.update(order);
            return RedirectToAction("index");
        }
    }
}
