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
        public decimal ListPrice { get; set; }

        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Range(1, 10000)]
        public decimal Price50 { get; set; }

        [Range(1, 10000)]
        public decimal Price100 { get; set; }

        public string ImageUrl { get; set; }

        public int CategoryId { get; set; }

        [Required]
        public Category Category { get; set; }

        public int CoverTypeId { get; set; }

        [Required]
        public CoverType CoverType { get; set; }
    }
}
