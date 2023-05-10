using BulkyBook.Core;
using BulkyBook.Core.Models;
using BulkyBook.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
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
            var product = _unitOfWork.Products.Get(p => p.Id == id);

            if (product == null)
                return NotFound();

            var viewModel = new ProductFormViewModel
            {
                Product = product,
                Categories = _unitOfWork.Categories.GetAll(),
                CoverTypes = _unitOfWork.CoverTypes.GetAll(),
                Heading = MethodBase.GetCurrentMethod().Name,
                IsEdit = true
            };

            return View("ProductForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(ProductFormViewModel viewModel, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return View("ProductForm", viewModel);
            
            if (file != null)
            {
                var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\products");

                if (viewModel.Product.ImageUrl != null) 
                {
                    string oldFileName = Path.GetFileName(viewModel.Product.ImageUrl);
                    string oldFilePath = Path.Combine(uploadPath, oldFileName);

                    if (System.IO.File.Exists(oldFilePath))
                        System.IO.File.Delete(oldFilePath);
                }
               
                var newFileName = Guid.NewGuid().ToString();
                
                var extension = Path.GetExtension(file.FileName);

                using var fileStream = new FileStream(Path.Combine(uploadPath, newFileName + extension), FileMode.Create);
                file.CopyTo(fileStream);

                viewModel.Product.ImageUrl = @"\images\products\" + newFileName + extension;
            }

            if (viewModel.Product.Id == 0)
            {
                _unitOfWork.Products.Add(viewModel.Product);
                _unitOfWork.Complete();

                TempData["success"] = "Product is created successfully";
            }
            else
            {
                _unitOfWork.Products.Update(viewModel.Product);
                _unitOfWork.Complete();

                TempData["success"] = "Product is updated successfully";
            }

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _unitOfWork.Products.GetAll(includeProperties:new List<string> { "Category", "CoverType"});
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var product = _unitOfWork.Products.Get(p => p.Id == id);

            if (product == null)
                return Json(new { success = false, message = "Unable to complete product deletion" });

            var fileDirectory = Path.Combine(_webHostEnvironment.WebRootPath, @"images\products");

            string fileName = Path.GetFileName(product.ImageUrl);
            string filePath = Path.Combine(fileDirectory, fileName);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            _unitOfWork.Products.Remove(product);
            _unitOfWork.Complete();

            return Json(new { success = true, message = "Product has been successfully deleted" });
        }

        #endregion 
    }
}
