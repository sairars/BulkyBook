using BulkyBook.Core;
using BulkyBook.Core.Models;
using BulkyBook.Core.ViewModels;
using BulkyBook.Utility;
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

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }

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

            ShoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = shoppingCartItems,
                Order = new()
            };

            foreach (var item in ShoppingCartViewModel.ShoppingCartItems)
            { 
                item.Price = GetPriceBasedOn(item.Quantity, item.Product);
                ShoppingCartViewModel.Order.Total += item.Price * item.Quantity;
            }

            return View(ShoppingCartViewModel);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartItems = _unitOfWork.ShoppingCartItems.GetAll(filter: sc => sc.UserId == userId,
                                                                            includeProperties: new List<string> { "Product" });

            ShoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = shoppingCartItems,
                Order = new()
            };

            ShoppingCartViewModel.Order.User = _unitOfWork.Users.Get(u => u.Id == userId);

            ShoppingCartViewModel.Order.Name = ShoppingCartViewModel.Order.User.Name;
            ShoppingCartViewModel.Order.PhoneNumber = ShoppingCartViewModel.Order.User.PhoneNumber;
            ShoppingCartViewModel.Order.StreetAddress = ShoppingCartViewModel.Order.User.StreetAddress;
            ShoppingCartViewModel.Order.City = ShoppingCartViewModel.Order.User.City;
            ShoppingCartViewModel.Order.State = ShoppingCartViewModel.Order.User.State;
            ShoppingCartViewModel.Order.PostalCode = ShoppingCartViewModel.Order.User.PostalCode;
                
            foreach (var item in ShoppingCartViewModel.ShoppingCartItems)
            {
                item.Price = GetPriceBasedOn(item.Quantity, item.Product);
                ShoppingCartViewModel.Order.Total += item.Price * item.Quantity;
            }

            return View(ShoppingCartViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public IActionResult SummaryPost()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartViewModel.ShoppingCartItems = _unitOfWork.ShoppingCartItems.GetAll(filter: sc => sc.UserId == userId,
                                                                            includeProperties: new List<string> { "Product" });
            ShoppingCartViewModel.Order.Status = StaticDetails.StatusPending;
            ShoppingCartViewModel.Order.PaymentStatus = StaticDetails.PaymentStatusPending;
            ShoppingCartViewModel.Order.CreationDate = DateTime.Now;
            ShoppingCartViewModel.Order.UserId = userId;

            foreach (var item in ShoppingCartViewModel.ShoppingCartItems)
            {
                item.Price = GetPriceBasedOn(item.Quantity, item.Product);
                ShoppingCartViewModel.Order.Total += item.Price * item.Quantity;
            }

            _unitOfWork.Orders.Add(ShoppingCartViewModel.Order);
            _unitOfWork.Complete();

            foreach (var item in ShoppingCartViewModel.ShoppingCartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = ShoppingCartViewModel.Order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };

                _unitOfWork.OrderDetails.Add(orderDetail);
            }

            _unitOfWork.Complete();
            _unitOfWork.ShoppingCartItems.RemoveRange(ShoppingCartViewModel.ShoppingCartItems);
            _unitOfWork.Complete();

            return RedirectToAction(nameof(Index), "Home");
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
