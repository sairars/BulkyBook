using BulkyBook.Core;
using BulkyBook.Core.Models;
using BulkyBook.Core.ViewModels;
using BulkyBook.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartItems = _unitOfWork.ShoppingCartItems.GetAll(filter: sc => sc.UserId == userId, 
                                                                            includeProperties: new List<string> { "Product"});

            var viewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = shoppingCartItems
            };

            foreach (var item in viewModel.ShoppingCartItems)
            { 
                item.Price = GetPriceBasedOn(item.Quantity, item.Product);
                viewModel.ShoppingCartTotal += item.Price * item.Quantity;
            }

            return View(viewModel);
        }

        public IActionResult Summary()
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var shoppingCartItems = _unitOfWork.ShoppingCartItems.GetAll(filter: sc => sc.UserId == userId,
            //                                                                includeProperties: new List<string> { "Product" });

            //var viewModel = new ShoppingCartViewModel
            //{
            //    ShoppingCartItems = shoppingCartItems
            //};

            //foreach (var item in viewModel.ShoppingCartItems)
            //{
            //    item.Price = GetPriceBasedOn(item.Quantity, item.Product);
            //    viewModel.ShoppingCartTotal += item.Price * item.Quantity;
            //}

            //return View(viewModel);

            return View();
        }

        public IActionResult Modify(int id, int change)
        {
            var shoppingCartItem = _unitOfWork.ShoppingCartItems.Get(sc => sc.Id == id);

            if (shoppingCartItem == null)
                return NotFound();

            if (shoppingCartItem.Quantity+ change == 0 || change == 0)
                _unitOfWork.ShoppingCartItems.Remove(shoppingCartItem);
            else
                _unitOfWork.ShoppingCartItems.ModifyQuantity(shoppingCartItem, change);
            
            _unitOfWork.Complete();
            
            return RedirectToAction(nameof(Index));
        }

        private static decimal GetPriceBasedOn(int quantity, Product product)
        {
            if (quantity <= 50)
                return product.Price;
            else if (quantity <= 100)
                return product.Price50;
            else
                return product.Price100;
        }
    }
}
