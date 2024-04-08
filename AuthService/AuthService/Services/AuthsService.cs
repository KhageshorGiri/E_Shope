using AuthService.DataContext;
using AuthService.Dtos;
using AuthService.Models;
using AuthService.Services.IService;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services
{
    public class AuthsService : IAuthService
    {
        private readonly AuthServiceDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly ILogger<AuthsService> _logger;

        public AuthsService(AuthServiceDbContext db, IJwtTokenProvider jwtTokenProvider,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            ILogger<AuthsService> logger)
        {
            _db = db;
            _jwtTokenProvider = jwtTokenProvider;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            _logger.LogInformation("Accept Request for AsignRoles with email {0} and role {1}", email, roleName);
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                _logger.LogInformation("Request Completed for AsignRoles with email {0} and role {1}", email, roleName);
                return true;
            }
            _logger.LogInformation("Cannot Completed Request for AsignRoles with email {0} and role {1}", email, roleName);
            return false;

        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            _logger.LogInformation("Accept Request for AsignRoles with {0}", loginRequestDto);
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            //if user was found , Generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenProvider.GenerateToken(user, roles);

            UserDto userDTO = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDTO,
                Token = token
            };
            _logger.LogInformation("Request Completed for AsignRoles with {0}", loginRequestDto);
            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            _logger.LogInformation("Accept Request for Register new User with {0}", registrationRequestDto);
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    _logger.LogInformation("Request Completed for Register new User with {0}", result);
                    return "";

                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
