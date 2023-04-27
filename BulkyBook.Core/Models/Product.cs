using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ISBN { get; set; }

        public string Author { get; set; }

        [Range(1, 10000)]
        [Display(Name = "List Price")]
        public decimal ListPrice { get; set; }

        [Range(1, 10000)]
        [Display(Name = "Price (1-50)")]
        public decimal Price { get; set; }

        [Range(1, 10000)]
        [Display(Name = "Price (51-100)")]
        public decimal Price50 { get; set; }

        [Range(1, 10000)]
        [Display(Name = "Price (100+)")]
        public decimal Price100 { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ValidateNever]
        public Category Category { get; set; }

        [Display(Name = "Cover Type")]
        public int CoverTypeId { get; set; }

        [ValidateNever]
        public CoverType CoverType { get; set; }
    }
}
