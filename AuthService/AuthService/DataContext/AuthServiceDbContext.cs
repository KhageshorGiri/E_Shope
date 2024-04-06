using AuthService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DataContext
{
    public class AuthServiceDbContext : IdentityDbContext<ApplicationUser>
    {
        public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options)
            :base(options)
        {
            
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
