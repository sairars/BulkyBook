using BulkyBook.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers.api
{
    [Route("/admin/api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompaniesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            var companies = _unitOfWork.Companies.GetAll();
            return Ok(companies);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) 
        {
            var company = _unitOfWork.Companies.Get(c => c.Id == id);

            if (company == null) 
                return NotFound();

            _unitOfWork.Companies.Remove(company);
            _unitOfWork.Complete();
            return Ok("Company deleted successfully");
        }
    }
}
