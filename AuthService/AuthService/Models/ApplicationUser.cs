using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
    }
}
