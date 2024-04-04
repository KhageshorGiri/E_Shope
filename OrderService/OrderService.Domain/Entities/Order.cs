using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.Entities
{
    public class Order : BaseEntity
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
