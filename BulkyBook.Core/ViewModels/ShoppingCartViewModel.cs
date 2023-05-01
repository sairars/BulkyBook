using BulkyBook.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Core.ViewModels
{
    public class ShoppingCartViewModel
    {
        [Range(1, 1000, ErrorMessage = "Must be between 1 and 1000")]
        public int Count { get; set; }
        public Product Product { get; set; }
    }
}
