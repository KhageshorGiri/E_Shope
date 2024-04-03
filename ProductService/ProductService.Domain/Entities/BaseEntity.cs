using System.ComponentModel.DataAnnotations;

namespace ProductService.Domain.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }

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
