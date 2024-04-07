using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TokenValidatorController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TokenValidatorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult ValidateToken()
        {
            string token = HttpContext.Request.Headers["Authorization"].ToString();

            if(string.IsNullOrWhiteSpace(token)) 
            {
                return BadRequest("Token is missing");
            }
            try
            {
                var key = _configuration.GetSection("ApiSettings:JwtOptions:Secret").Value;
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuer = false,
                    //ValidIssuer = _configuration.GetSection("ApiSettings:JwtOptions:Issuer").Value,
                    ValidateAudience = false,
                    //ValidAudience = _configuration.GetSection("ApiSettings:JwtOptions:Audience").Value,
                    ValidateLifetime = true
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token.Replace("Bearer ", ""), validationParameters, out validatedToken);

                
                // Token is valid
                return Ok("Token is valid");
            }
            catch (SecurityTokenValidationException ex)
            {
                return BadRequest("Token validation failed: " + ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Token validation failed: " + ex.Message);
            }
        }
    }
}
