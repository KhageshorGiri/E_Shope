using System.ComponentModel.DataAnnotations;

namespace ProductService.Domain.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? ProductDescription { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
