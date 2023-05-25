using BulkyBook.Core;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BulkyBookWeb.Areas.Admin.Controllers.api
{
    [Route("/admin/api/[controller]")]
    [ApiController]
    [Authorize(Roles = StaticDetails.Admin)]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = _unitOfWork.Products.GetAll(includeProperties:new List<string> { "Category", "CoverType" });
            return Ok(products);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _unitOfWork.Products.Get(p => p.Id == id);

            if (product == null)
                return NotFound("Product could not be deleted");

            var fileDirectory = Path.Combine(_webHostEnvironment.WebRootPath, @"images\products");

            string fileName = Path.GetFileName(product.ImageUrl);
            string filePath = Path.Combine(fileDirectory, fileName);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            _unitOfWork.Products.Remove(product);
            _unitOfWork.Complete();

            return Ok("Product deleted successfully");
        }
    }
}
