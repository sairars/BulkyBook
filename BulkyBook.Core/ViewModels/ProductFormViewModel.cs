using BulkyBook.Core.Models;

namespace BulkyBook.Core.ViewModels
{
    public class ProductFormViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<CoverType> CoverTypes { get; set; }
        public string Heading { get; set; }
        public bool IsEdit { get; set; }
    }
}
