using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View("CategoryForm");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
                ModelState.AddModelError("DisplayOrder", "Display Order cannot be same as Category Name");

            if (!ModelState.IsValid)
                return View("CategoryForm", category);

            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
