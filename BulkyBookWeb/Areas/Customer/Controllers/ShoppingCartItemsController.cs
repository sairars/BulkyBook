using BulkyBook.Core;
using BulkyBook.Core.Models;
using BulkyBook.Core.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartItemsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ShoppingCartItemsController(IUnitOfWork unitOfWork, IEmailSender emailSender, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartItems = _unitOfWork.ShoppingCartItems.GetAll(filter: sc => sc.UserId == userId, 
                                                                            includeProperties: new List<string> { "Product"});

            var viewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = shoppingCartItems,
                Order = new()
            };

            foreach (var item in viewModel.ShoppingCartItems)
            { 
                item.Price = GetPriceBasedOn(item.Quantity, item.Product);
                viewModel.Order.Total += item.Price * item.Quantity;
            }

            return View(viewModel);
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var shoppingCartItems = _unitOfWork.ShoppingCartItems.GetAll(filter: sc => sc.UserId == userId,
                                                                            includeProperties: new List<string> { "Product" });

            var viewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = shoppingCartItems,
                Order = new()
            };

            var user = _unitOfWork.Users.Get(u => u.Id == userId);

            viewModel.Order.Name = user.Name;
            viewModel.Order.PhoneNumber = user.PhoneNumber;
            viewModel.Order.StreetAddress = user.StreetAddress;
            viewModel.Order.City = user.City;
            viewModel.Order.State = user.State;
            viewModel.Order.PostalCode = user.PostalCode;

            foreach (var item in viewModel.ShoppingCartItems)
            {
                item.Price = GetPriceBasedOn(item.Quantity, item.Product);
                viewModel.Order.Total += item.Price * item.Quantity;
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PlaceOrder(ShoppingCartViewModel viewModel)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            viewModel.ShoppingCartItems = _unitOfWork.ShoppingCartItems.GetAll(filter: sc => sc.UserId == userId,
                                                                            includeProperties: new List<string> { "Product" });
            viewModel.Order.Status = User.IsInRole(StaticDetails.CompanyUser)
                                                        ? StaticDetails.StatusApproved 
                                                        : StaticDetails.StatusPending;

            viewModel.Order.PaymentStatus = User.IsInRole(StaticDetails.CompanyUser) 
                                                        ? StaticDetails.PaymentStatusDelayedPayment 
                                                        : StaticDetails.PaymentStatusPending;
            viewModel.Order.CreationDate = DateTime.Now;
            viewModel.Order.UserId = userId;

            foreach (var item in viewModel.ShoppingCartItems)
            {
                item.Price = GetPriceBasedOn(item.Quantity, item.Product);
                viewModel.Order.Total += item.Price * item.Quantity;
            }

            _unitOfWork.Orders.Add(viewModel.Order);
            _unitOfWork.Complete();

            foreach (var item in viewModel.ShoppingCartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = viewModel.Order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };

                _unitOfWork.OrderDetails.Add(orderDetail);
            }

            _unitOfWork.Complete();

            if (User.IsInRole(StaticDetails.CompanyUser))
                return RedirectToAction(nameof(OrderConfirmation), routeValues: new { viewModel.Order.Id });

            // stripe settings
            var domain = (_webHostEnvironment.IsDevelopment()) 
                                    ? "https://localhost:44328/" 
                                    : "https://bulky1.azurewebsites.net/";
            

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{domain}customer/ShoppingCartItems/OrderConfirmation?id={viewModel.Order.Id}",
                CancelUrl = $"{domain}customer/ShoppingCartItems/Index"
            };

            foreach (var item in viewModel.ShoppingCartItems)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = item.Quantity
                };

                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);

            _unitOfWork.Orders.UpdateStripeSessionId(viewModel.Order.Id, session.Id);
            _unitOfWork.Complete();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult OrderConfirmation(int id)
        {
            var order = _unitOfWork.Orders.Get(o => o.Id == id, includeProperties: new List<string> { "User"});

            if (order == null) 
                return NotFound();

            if (order.PaymentStatus != StaticDetails.PaymentStatusDelayedPayment)
            {
                var service = new SessionService();
                var session = service.Get(order.SessionId);

                // check the stripe status
                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.Orders.UpdateStripePaymentId(id, session.PaymentIntentId);
                    _unitOfWork.Orders.UpdateStatus(id, StaticDetails.StatusApproved, StaticDetails.PaymentStatusApproved);
                    _unitOfWork.Complete();
                }
            }

            var shoppingCartItems = _unitOfWork.ShoppingCartItems.GetAll(filter: sc => sc.UserId == order.UserId);
            _unitOfWork.ShoppingCartItems.RemoveRange(shoppingCartItems);
            _unitOfWork.Complete();

            HttpContext.Session.Clear();

            _emailSender.SendEmailAsync(order.User.Email, "New Order - Bulky Books", "A new order has been successfully created");

            return View(id);
        }

        public IActionResult Modify(int id, int change)
        {
            var shoppingCartItem = _unitOfWork.ShoppingCartItems.Get(sc => sc.Id == id);

            if (shoppingCartItem == null)
                return NotFound();

            if (shoppingCartItem.Quantity + change == 0 || change == 0)
            {
                _unitOfWork.ShoppingCartItems.Remove(shoppingCartItem);

                var numberOfShoppingCartItems = _unitOfWork.ShoppingCartItems
                                                            .GetAll(sc => sc.UserId == shoppingCartItem.UserId)
                                                            .ToList()
                                                            .Count();
                
               HttpContext.Session.SetInt32(StaticDetails.SessionShoppingCart, numberOfShoppingCartItems-1);
            }
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
