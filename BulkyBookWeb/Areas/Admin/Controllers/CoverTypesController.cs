using BulkyBook.Core;
using BulkyBook.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var coverTypes = _unitOfWork.CoverTypes.GetAll();
            return View(coverTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var coverTypes = _unitOfWork.CoverTypes.Get(c => c.Id == id);

            if (coverTypes == null)
                return NotFound();

            return View(coverTypes);
        }

        public IActionResult Delete(int id)
        {
            var coverTypes = _unitOfWork.CoverTypes.Get(c => c.Id == id);

            if (coverTypes == null)
                return NotFound();

            return View(coverTypes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            if (!ModelState.IsValid)
                return View(coverType);

            _unitOfWork.CoverTypes.Add(coverType);
            _unitOfWork.Complete();

            TempData["success"] = "Cover Type is created successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(CoverType coverType)
        {
            if (!ModelState.IsValid)
                return View(coverType);

            _unitOfWork.CoverTypes.Update(coverType);
            _unitOfWork.Complete();

            TempData["success"] = "Cover Type is updated successfully";

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCoverType(int id)
        {
            var coverType = _unitOfWork.CoverTypes.Get(c => c.Id == id);

            if (coverType == null)
                return NotFound();

            _unitOfWork.CoverTypes.Remove(coverType);
            _unitOfWork.Complete();

            TempData["success"] = "Cover Type is deleted successfully";

            return RedirectToAction("Index");
        }
    }
}
