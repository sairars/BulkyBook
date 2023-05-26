using BulkyBook.Core;
using BulkyBook.Core.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.ViewComponents
{
    public class ShoppingCartItemsViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartItemsViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                HttpContext.Session.Clear();
                return View(0);
            }

            var sessionShoppingCart = HttpContext.Session.GetInt32(StaticDetails.SessionShoppingCart);

            if (sessionShoppingCart != null) 
                return View(sessionShoppingCart.Value);
            
            var userId = claim.Value;
            var numberOfShoppingCartItems = _unitOfWork.ShoppingCartItems
                                                        .GetAll(sc => sc.UserId == userId)
                                                        .ToList()
                                                        .Count();
            HttpContext.Session.SetInt32(StaticDetails.SessionShoppingCart, numberOfShoppingCartItems);
            return View(numberOfShoppingCartItems);
        }
    }
}
