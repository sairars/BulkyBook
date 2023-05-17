using BulkyBook.Core;
using BulkyBook.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

            return RedirectToAction("Details", routeValues: new { id = order.Id });
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
