using BulkyBook.Core;
using BulkyBook.Core.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Admin.Controllers.api
{
    [Route("Admin/api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("{status}")]
        public IActionResult GetAllOrders(string status)
        {
            var orders = _unitOfWork.Orders.GetAll(includeProperties: new List<string> { "User" });

            if (!User.IsInRole(StaticDetails.Admin) && !User.IsInRole(StaticDetails.Staff))
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                orders = orders.Where(o => o.UserId == userId);
            }

            if (status == "paymentpending")
                orders = orders.Where(o => o.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment);
            else if (status == "approved")
                orders = orders.Where(o => o.Status == StaticDetails.StatusApproved);
            else if (status == "inprocess")
                orders = orders.Where(o => o.Status == StaticDetails.StatusInProcess);
            else if (status == "completed")
                orders = orders.Where(o => o.Status == StaticDetails.StatusShipped);

            return Ok(orders);
        }
    }
}
