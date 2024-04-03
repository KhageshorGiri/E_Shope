using System.ComponentModel.DataAnnotations;

namespace ProductService.Application.Dtos
{
    public record ProductDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateProductDto
    {
        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? ProductDescription { get; set; }

        [Required]
        public decimal Price { get; set; }
    }

    public class UpdateProductDto
    {
        [Required]
        public string? ProductName { get; set; }

        [Required]
        public string? ProductDescription { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
