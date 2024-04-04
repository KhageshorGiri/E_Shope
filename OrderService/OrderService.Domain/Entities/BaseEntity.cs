using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        [Required]
        public int CreatedBy { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int ModifiedBy { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ModifiedDate { get; set; }
    }
}
