using Microsoft.AspNetCore.Mvc;
using TemuDB.API.Models;
using TemuDB.API.Services;

namespace TemuDB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _authService.AuthenticateUser(request.Username, request.Password);

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
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _authService.RegisterUser(request);

            if (success)
            {
                return Ok(new { success = true, message = "Registrierung erfolgreich. Bitte warten Sie auf die Freischaltung durch den Administrator." });
            }

            return BadRequest(new { success = false, message = "Benutzername bereits vergeben" });
        }

        [HttpGet("users/inactive")]
        public IActionResult GetInactiveUsers()
        {
            var inactiveUsers = _authService.GetInactiveUsers();
            return Ok(inactiveUsers);
        }

        [HttpGet("users/all")]
        public IActionResult GetAllUsers()
        {
            var allUsers = _authService.GetAllUsers();
            return Ok(allUsers);
        }

        [HttpPost("users/{userId}/activate")]
        public IActionResult ActivateUser(int userId)
        {
            var success = _authService.ActivateUser(userId);

            if (success)
            {
                return Ok(new { success = true, message = "Benutzer erfolgreich aktiviert" });
            }

            return NotFound(new { success = false, message = "Benutzer nicht gefunden" });
        }
    }
}
