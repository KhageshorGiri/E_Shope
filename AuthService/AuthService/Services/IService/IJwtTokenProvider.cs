using AuthService.Models;

namespace AuthService.Services.IService
{
    public interface IJwtTokenProvider
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
    }
}
