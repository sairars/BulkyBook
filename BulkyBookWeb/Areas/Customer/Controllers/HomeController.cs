using BulkyBook.Core;
using BulkyBook.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Products.GetAll();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _unitOfWork.Products.Get( p => p.Id == id, 
                                                    includeProperties:new List<string> { "Category", "CoverType" });

            if (product == null)
                return NotFound();

            var viewModel = new ShoppingCartViewModel
            {
                Count = 1,
                Product = product
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}