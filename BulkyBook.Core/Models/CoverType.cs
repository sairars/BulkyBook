using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Core.Models
{
    public class CoverType
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Cover Type")]
        public string? Name { get; set; }
    }
}
