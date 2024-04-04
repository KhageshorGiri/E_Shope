using System.ComponentModel.DataAnnotations;

namespace OrderService.Application.Dtos
{
    public record OrderDto
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? OrderDescription { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderDeliveryDate { get; set; }
    }

    public record CreateOrderDto
    {
        [Required]
        [MaxLength(200)]
        public string? ProductName { get; set; }

        [Required]
        [MaxLength(500)]
        public string? OrderDescription { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDeliveryDate { get; set; }
    }

    public record UpdateOrderDto
    {
        [Required]
        [MaxLength(200)]
        public string? ProductName { get; set; }

        [Required]
        [MaxLength(500)]
        public string? OrderDescription { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderDeliveryDate { get; set; }
    }
}
