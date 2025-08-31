using Microsoft.AspNetCore.Mvc;
using TemuDB.API.Models;
using TemuDB.API.Services;

namespace TemuDB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthServiceEF _authService;

        public AuthController(AuthServiceEF authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _authService.AuthenticateUserAsync(request.Username, request.Password);

            if (user != null)
            {
                return Ok(new
                {
                    success = true,
                    user = new
                    {
                        id = user.Id,
                        username = user.Username,
                        displayName = user.DisplayName,
                        isAdmin = user.IsAdmin
                    }
                });
            }

            return Unauthorized(new { success = false, message = "Ungültige Anmeldedaten" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _authService.RegisterUserAsync(request);

            if (success)
            {
                return Ok(new { success = true, message = "Registrierung erfolgreich. Bitte warten Sie auf die Freischaltung durch den Administrator." });
            }

            return BadRequest(new { success = false, message = "Benutzername bereits vergeben" });
        }

        [HttpGet("users/inactive")]
        public async Task<IActionResult> GetInactiveUsers()
        {
            var inactiveUsers = await _authService.GetInactiveUsersAsync();
            return Ok(inactiveUsers);
        }

        [HttpGet("users/all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var allUsers = await _authService.GetAllUsersAsync();
            return Ok(allUsers);
        }

        [HttpPost("users/{userId}/activate")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            var success = await _authService.ActivateUserAsync(userId);

            if (success)
            {
                return Ok(new { success = true, message = "Benutzer erfolgreich aktiviert" });
            }

            return NotFound(new { success = false, message = "Benutzer nicht gefunden" });
        }
    }
}
