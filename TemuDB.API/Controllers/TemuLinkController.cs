using Microsoft.AspNetCore.Mvc;
using TemuDB.API.Models;
using TemuDB.API.Services;

namespace TemuDB.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemuLinkController : ControllerBase
    {
        private readonly TemuLinkService _temuLinkService;

        public TemuLinkController(TemuLinkService temuLinkService)
        {
            _temuLinkService = temuLinkService;
        }

        [HttpGet("user/{username}")]
        public IActionResult GetTemuLinksByUser(string username)
        {
            var links = _temuLinkService.GetTemuLinksByUser(username);
            return Ok(links);
        }

        [HttpPost]
        public IActionResult AddTemuLink([FromBody] TemuLinkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var temuLink = _temuLinkService.AddTemuLink(request.Username, request.Description, request.Link, request.IsPublic);

            if (temuLink != null)
            {
                return Ok(new { success = true, temuLink });
            }

            return BadRequest(new { success = false, message = "Fehler beim Speichern des Links" });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTemuLink(int id, [FromBody] TemuLinkRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var temuLink = _temuLinkService.UpdateTemuLink(id, request.Username, request.Description, request.Link, request.IsPublic);

            if (temuLink != null)
            {
                return Ok(new { success = true, temuLink });
            }

            return NotFound(new { success = false, message = "Link nicht gefunden oder keine Berechtigung" });
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTemuLink(int id, [FromQuery] string username)
        {
            var success = _temuLinkService.DeleteTemuLink(id, username);

            if (success)
            {
                return Ok(new { success = true, message = "Link erfolgreich gelöscht" });
            }

            return NotFound(new { success = false, message = "Link nicht gefunden oder keine Berechtigung" });
        }

        [HttpGet("all")]
        public IActionResult GetAllTemuLinks()
        {
            var links = _temuLinkService.GetAllTemuLinks();
            return Ok(links);
        }

        [HttpGet("public")]
        public IActionResult GetPublicTemuLinks()
        {
            var links = _temuLinkService.GetPublicTemuLinks();
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
