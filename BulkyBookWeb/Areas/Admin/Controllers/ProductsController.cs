using BulkyBook.Core;
using BulkyBook.Core.Models;
using BulkyBook.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Products.GetAll();
            return View(products);
        }

        public IActionResult Create()
        {
            var viewModel = new ProductFormViewModel
            {
                Categories = _unitOfWork.Categories.GetAll(),
                CoverTypes = _unitOfWork.CoverTypes.GetAll(),
                Heading = MethodBase.GetCurrentMethod().Name,
                IsEdit = false
            };

            return View("ProductForm", viewModel);
        }

        public IActionResult Edit(int id)
        {
            var category = _unitOfWork.Categories.Get(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var category = _unitOfWork.Categories.Get(id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductFormViewModel viewModel, IFormFile file)
        {
            if (!ModelState.IsValid)
                return View("ProductFormViewModel", viewModel);

            //_unitOfWork.Products.Add(category);
            //_unitOfWork.Complete();

            //TempData["success"] = "Product is created successfully";

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
            var category = _unitOfWork.Categories.Get(id);

            if (category == null)
                return NotFound();

            _unitOfWork.Categories.Remove(category);
            _unitOfWork.Complete();

            TempData["success"] = "Category is deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
