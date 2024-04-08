using AuthService.Dtos;
using AuthService.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        protected ResponseDto _response;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
            _response = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            _logger.LogInformation("Get request For Regitration with {0}", JsonSerializer.Serialize(model));
            if (!ModelState.IsValid)
            {
                _response.Message = ModelState.ToString();
                return BadRequest(_response);
            }

            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage))
            {
                _response.IsSuccess = false;
                _response.Message = errorMessage;
                _logger.LogInformation("Request For Regitration completed with {0}", errorMessage);
                return BadRequest(_response);
            }

            _logger.LogInformation("Request For Regitration for {0} Completed with {1}", JsonSerializer.Serialize(model), _response.ToString());
            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            _logger.LogInformation("Get request For Login for {0}", JsonSerializer.Serialize(model));
            if (!ModelState.IsValid)
            {
                _response.Message = ModelState.ToString();
                return BadRequest(_response);
            }

            var loginResponse = await _authService.Login(model);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                _logger.LogInformation("Request For Login completed for {0} with {1}", JsonSerializer.Serialize(model), _response.Message);
                return BadRequest(_response);
            }

            _response.Result = loginResponse;
            _logger.LogInformation("Request For Login for {0} Completed with {1}", JsonSerializer.Serialize(model), loginResponse);
            return Ok(_response);
        }

        [Authorize]
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            _logger.LogInformation("Get request For AssignRole for {0}", JsonSerializer.Serialize(model));
            if (!ModelState.IsValid)
            {
                _response.Message = ModelState.ToString();
                return BadRequest(_response);
            }

            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
                _logger.LogInformation("Completed request For Login completed with {0}", assignRoleSuccessful);
                return BadRequest(_response);
            }

            _logger.LogInformation("Request For AssignRole for {0} Completed with {1}", JsonSerializer.Serialize(model), _response);
            return Ok(_response);
        }

    }
}
