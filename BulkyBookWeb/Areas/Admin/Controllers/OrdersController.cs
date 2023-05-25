using BulkyBook.Core;
using BulkyBook.Core.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var order = _unitOfWork.Orders.Get(filter: o => o.Id == id, 
                                                includeProperties:new List<string> { "User" });

            if (order == null)
                return NotFound();

            var orderDetails = _unitOfWork.OrderDetails.GetAll(filter: od => od.OrderId == id,
                                                                includeProperties: new List<string> { "Product" });

            var viewModel = new OrderViewModel
            {
                Order = order,
                OrderDetails = orderDetails
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = StaticDetails.Admin + "," + StaticDetails.Staff)]
        public IActionResult Update(OrderViewModel viewModel)
        {
            var order = viewModel.Order;
            var orderDb = _unitOfWork.Orders.Get(o => o.Id == order.Id, tracked: false);

            if (orderDb == null)
                return NotFound();  

            orderDb.Name = order.Name;
            orderDb.PhoneNumber = order.PhoneNumber;
            orderDb.StreetAddress = order.StreetAddress;
            orderDb.City = order.City;
            orderDb.State = order.State;
            orderDb.PostalCode = order.PostalCode;
            
            if (order.Carrier != null)
                orderDb.Carrier = order.Carrier;

            if (order.TrackingNumber != null)
                orderDb.TrackingNumber = order.TrackingNumber;

            _unitOfWork.Orders.Update(orderDb);
            _unitOfWork.Complete();

            TempData["success"] = "Order details updated successfully";

            return RedirectToAction(nameof(Details), routeValues: new { id = order.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =  StaticDetails.Admin + "," + StaticDetails.Staff)]
        public IActionResult Process(OrderViewModel viewModel) 
        {
            var id = viewModel.Order.Id;

            _unitOfWork.Orders.UpdateStatus(id, StaticDetails.StatusInProcess);
            _unitOfWork.Complete();

            TempData["success"] = "Order Is Being Processed";

            return RedirectToAction(nameof(Details), routeValues: new { id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = StaticDetails.Admin + "," + StaticDetails.Staff)]
        public IActionResult Ship(OrderViewModel viewModel)
        {
            var order = viewModel.Order;
            var orderDb = _unitOfWork.Orders.Get(o => o.Id == order.Id, tracked: false);

            if (orderDb == null)
                return NotFound();

            orderDb.TrackingNumber = order.TrackingNumber;
            orderDb.Carrier = order.Carrier;
            orderDb.ShippingDate = DateTime.Now;

            if (orderDb.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment)
                orderDb.PaymentDueDate = DateTime.Now.AddDays(30);

            _unitOfWork.Orders.Update(orderDb);
            
            _unitOfWork.Orders.UpdateStatus(order.Id, StaticDetails.StatusShipped);
            
            _unitOfWork.Complete();

            TempData["success"] = "Order has been shipped successfully";

            return RedirectToAction(nameof(Details), routeValues: new { order.Id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = StaticDetails.Admin + "," + StaticDetails.Staff)]
        public IActionResult Cancel(OrderViewModel viewModel)
        {
            var order = viewModel.Order;

            if (order.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment)
                _unitOfWork.Orders.UpdateStatus(order.Id, StaticDetails.StatusCancelled, StaticDetails.StatusCancelled);
            else
            {
                var options = new RefundCreateOptions
                {
                    PaymentIntent = order.PaymentIntentId
                };
                
                var service = new RefundService();
                var refund = service.Create(options);
                _unitOfWork.Orders.UpdateStatus(order.Id, StaticDetails.StatusCancelled, StaticDetails.StatusRefunded);

            }
            _unitOfWork.Complete();
            return RedirectToAction(nameof(Details), routeValues: new { order.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PayForOrder(OrderViewModel viewModel)
        {
            var order = viewModel.Order;
            var orderDetails = _unitOfWork.OrderDetails.GetAll(od => od.OrderId == order.Id, 
                                                                includeProperties: new List<string> { "Product"});

            // stripe settings
            var domain = "https://localhost:44328/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = $"{domain}admin/Orders/PaymentConfirmation?id={order.Id}",
                CancelUrl = $"{domain}admin/Orders/Details?id={order.Id}"
            };

            foreach (var item in orderDetails)
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

            _unitOfWork.Orders.UpdateStripeSessionId(order.Id, session.Id);
            _unitOfWork.Complete();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult PaymentConfirmation(int id)
        {
            var order = _unitOfWork.Orders.Get(o => o.Id == id);

            if (order == null)
                return NotFound();

            if (order.PaymentStatus != StaticDetails.PaymentStatusDelayedPayment)
                return RedirectToAction(nameof(Details), routeValues: new { id });

            var service = new SessionService();
            var session = service.Get(order.SessionId);

            // check the stripe status
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.Orders.UpdateStripePaymentId(id, session.PaymentIntentId);
                _unitOfWork.Orders.UpdateStatus(id, order.Status, StaticDetails.PaymentStatusApproved);
                _unitOfWork.Complete();
            }
            
            return View(id);
        }

        #region API CALLS

        public IActionResult GetAllOrders()
        {
            var orders = _unitOfWork.Orders.GetAll();
            return Json(new {data = orders});
        }

        #endregion
    }
}
