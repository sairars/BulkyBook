using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Core.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "{0} has to be in the range of {1} to {2}")]
        public int DisplayOrder { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
