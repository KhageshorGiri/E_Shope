using System.ComponentModel.DataAnnotations;

namespace AuthService.Dtos
{
    public class RegistrationRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Role { get; set; }
    }
}
