using BulkyBook.Core;
using BulkyBook.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompaniesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var company = new Company();
            return View("CompanyForm", company);
        }

        public IActionResult Edit(int id)
        {
            var company = _unitOfWork.Companies.Get(c => c.Id == id);

            if (company == null) 
                return NotFound();


            return View("CompanyForm", company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Company company)
        {
            if (!ModelState.IsValid)
                return View(company);

            if (company.Id == 0)
            {
                _unitOfWork.Companies.Add(company);
                _unitOfWork.Complete();
                TempData["success"] = "Company is created successfully";
            }
            else 
            {
                _unitOfWork.Companies.Update(company);
                _unitOfWork.Complete();
                TempData["success"] = "Company is updated successfully";
            }

            return RedirectToAction("Index");
        }

        #region API Calls

        [HttpPost]
        public IActionResult GetAllCompanies() 
        {
            var companies = _unitOfWork.Companies.GetAll();
            return Json(new { data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            var company = _unitOfWork.Companies.Get(c => c.Id == id);

            if (company == null)
                return Json(new { success = false, message = "Unable to complete company deletion" });

            _unitOfWork.Companies.Remove(company);
            _unitOfWork.Complete(); 

            return Json(new { success = true, message = "Company has been deleted successfully" });
        }
        #endregion
    }
}
