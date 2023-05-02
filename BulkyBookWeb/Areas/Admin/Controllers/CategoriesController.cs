using BulkyBook.Core;
using BulkyBook.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _unitOfWork.Categories.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var category = _unitOfWork.Categories.Get(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var category = _unitOfWork.Categories.Get(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
                ModelState.AddModelError("DisplayOrder", "Display Order cannot be same as Category Name");

            if (!ModelState.IsValid)
                return View(category);

            _unitOfWork.Categories.Add(category);
            _unitOfWork.Complete();

            TempData["success"] = "Category is created successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
                ModelState.AddModelError("DisplayOrder", "Display Order cannot be same as Category Name");

            if (!ModelState.IsValid)
                return View(category);

            _unitOfWork.Categories.Update(category);
            _unitOfWork.Complete();

            TempData["success"] = "Category is updated successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int id)
        {
            var category = _unitOfWork.Categories.Get(c => c.Id == id);

            if (category == null)
                return NotFound();

            _unitOfWork.Categories.Remove(category);
            _unitOfWork.Complete();

            TempData["success"] = "Category is deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
