using Microsoft.AspNetCore.Mvc;
using TemuDB.API.Models;
using TemuDB.API.Services;

namespace TemuDB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemuLinkController : ControllerBase
    {
        private readonly TemuLinkServiceEF _temuLinkService;

        public TemuLinkController(TemuLinkServiceEF temuLinkService)
        {
            _temuLinkService = temuLinkService;
        }

        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetTemuLinksByUser(string username)
        {
            var links = await _temuLinkService.GetTemuLinksByUserAsync(username);
            return Ok(links);
        }

        [HttpPost]
        public async Task<IActionResult> AddTemuLink([FromBody] TemuLinkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var temuLink = await _temuLinkService.AddTemuLinkAsync(request.Username, request.Description, request.Link, request.IsPublic);

            if (temuLink != null)
            {
                return Ok(new { success = true, temuLink });
            }

            return BadRequest(new { success = false, message = "Fehler beim Speichern des Links" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTemuLink(int id, [FromBody] TemuLinkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var temuLink = await _temuLinkService.UpdateTemuLinkAsync(id, request.Username, request.Description, request.Link, request.IsPublic);

            if (temuLink != null)
            {
                return Ok(new { success = true, temuLink });
            }

            return NotFound(new { success = false, message = "Link nicht gefunden oder keine Berechtigung" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemuLink(int id, [FromQuery] string username)
        {
            var success = await _temuLinkService.DeleteTemuLinkAsync(id, username);

            if (success)
            {
                return Ok(new { success = true, message = "Link erfolgreich gelöscht" });
            }

            return NotFound(new { success = false, message = "Link nicht gefunden oder keine Berechtigung" });
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTemuLinks()
        {
            var links = await _temuLinkService.GetAllTemuLinksAsync();
            return Ok(links);
        }

        [HttpGet("public")]
        public async Task<IActionResult> GetPublicTemuLinks()
        {
            var links = await _temuLinkService.GetPublicTemuLinksAsync();
            return Ok(links);
        }
    }

    public class TemuLinkRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
    }
}
