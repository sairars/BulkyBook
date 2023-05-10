using BulkyBook.Core;
using BulkyBook.Core.Models;
using BulkyBook.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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

            var shoppingCartItem = new ShoppingCartItem
            {
                ProductId = id,
                Quantity = 1,
                Product = product
            };

            return View(shoppingCartItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCartItem shoppingCartItem)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCartItem.UserId = userId;

            var shoppingCartItemFromDb = _unitOfWork.ShoppingCartItems.Get(sc =>
                                                                                sc.UserId == userId &&
                                                                                sc.ProductId == shoppingCartItem.ProductId);

            if (shoppingCartItemFromDb != null)
                _unitOfWork.ShoppingCartItems.ModifyQuantity(shoppingCartItemFromDb, shoppingCartItem.Quantity);
            else
                _unitOfWork.ShoppingCartItems.Add(shoppingCartItem);
            _unitOfWork.Complete();

             return RedirectToAction(nameof(Index));
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