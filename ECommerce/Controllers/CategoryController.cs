using System.Data;
using ECommerce.Models;
using ECommerce.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using X.PagedList;

namespace ECommerce.Controllers
{
    [Route("admin/category/[action]/{id?}")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IOperations<Category> category;
        private readonly IToastNotification toastNotification;

        public CategoryController(IOperations<Category> category, IToastNotification toastNotification)
        {
            this.category = category;
            this.toastNotification = toastNotification;
        }

        IPagedList<Category> Search(string search, int? page)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return null;
            }
            else
            {
                var res = category.Search(search).ToPagedList(page ?? 1, 10);
                return res;
            }

        }
        public IActionResult Index(int? page, string search)
        {
            if (search != null)
            {
                var listSort = Search(search, page);
                if (listSort != null)
                {
                    return View(listSort);
                }
            }

            var model = category.List().ToList().ToPagedList(page ?? 1, 10);
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                category.Add(model);
                toastNotification.AddSuccessToastMessage("تم اضافة الفئه");
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = category.Find(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                category.update(model);
                toastNotification.AddSuccessToastMessage("تم تعديل الفئه");
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id, string Sequre)
        {
            category.delete(id);
            toastNotification.AddSuccessToastMessage("تم حذف الفئه ");
            return RedirectToAction("index");
        }
    }
}
