using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Core.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        
        public Product Product { get; set; }

        [Range(1, 1000, ErrorMessage = "Must be between 1 and 1000")]
        public int Quantity { get; set; }
        
        public string UserId { get; set; }
        
        public ApplicationUser User { get; set; }

        [NotMapped]
        public decimal Price { get; set; }
    }
}
