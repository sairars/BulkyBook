using BulkyBook.Core.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BulkyBook.Core.ViewModels
{
    public class ProductFormViewModel
    {
        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<Category> Categories { get; set; }

        [ValidateNever]
        public IEnumerable<CoverType> CoverTypes { get; set; }

        [ValidateNever]
        public string Heading { get; set; }
        public bool IsEdit { get; set; }
    }
}
