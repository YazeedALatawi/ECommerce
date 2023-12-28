using ECommerce.Models.Repositories;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ECommerce.Controllers
{
    [Route("admin/shipping/[action]/{id?}")]
    [Authorize(Roles = "Admin")]

    public class ShippingCompaniesController : Controller
    {
        private readonly IOperations<Shipping> shipping;
        private readonly IToastNotification toastNotification;

        public ShippingCompaniesController(IOperations<Shipping> shipping, IToastNotification toastNotification)
        {
            this.shipping = shipping;
            this.toastNotification = toastNotification;
        }

        IPagedList<Shipping> Search(string search, int? page)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return null;
            }
            else
            {
                var res = shipping.Search(search).ToPagedList(page ?? 1, 10);
                return res;
            }

        }
        public IActionResult Index(int ? page, string search)
        {
            if (search != null)
            {
                var listSort = Search(search, page);
                if (listSort != null)
                {
                    return View(listSort);
                }
            }
            var model = shipping.List().ToList().ToPagedList(page ?? 1, 10);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Shipping model)
        {
            if (ModelState.IsValid)
            {
                shipping.Add(model);
                toastNotification.AddSuccessToastMessage("تم اضافة شركة الشحن");
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = shipping.Find(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Shipping model)
        {
            if (ModelState.IsValid)
            {
                shipping.update(model);
                toastNotification.AddSuccessToastMessage("تم تعديل شركة الشحن");
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id, string Sequre)
        {
            shipping.delete(id);
            toastNotification.AddSuccessToastMessage("تم حذف شركة الشحن ");
            return RedirectToAction("index");
        }
    }
}
