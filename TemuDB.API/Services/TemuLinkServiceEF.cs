using Microsoft.EntityFrameworkCore;
using TemuDB.API.Data;
using TemuDB.API.Models;

namespace TemuDB.API.Services
{
    public class TemuLinkServiceEF
    {
        private readonly ApplicationDbContext _context;

        public TemuLinkServiceEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TemuLink>> GetTemuLinksByUserAsync(string username)
        {
            return await _context.TemuLinks
                .Where(t => t.Username == username)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<TemuLink?> AddTemuLinkAsync(string username, string description, string link, bool isPublic = false)
        {
            var temuLink = new TemuLink
            {
                Username = username,
                Description = description,
                Link = link,
                IsPublic = isPublic,
                CreatedAt = DateTime.UtcNow
            };

            _context.TemuLinks.Add(temuLink);
            await _context.SaveChangesAsync();
            return temuLink;
        }

        public async Task<TemuLink?> UpdateTemuLinkAsync(int id, string username, string description, string link, bool isPublic)
        {
            var existingLink = await _context.TemuLinks
                .FirstOrDefaultAsync(t => t.Id == id && t.Username == username);

            if (existingLink != null)
            {
                existingLink.Description = description;
                existingLink.Link = link;
                existingLink.IsPublic = isPublic;

                await _context.SaveChangesAsync();
                return existingLink;
            }

            return null;
        }

        public async Task<bool> DeleteTemuLinkAsync(int id, string username)
        {
            var link = await _context.TemuLinks
                .FirstOrDefaultAsync(t => t.Id == id && t.Username == username);

            if (link != null)
            {
                _context.TemuLinks.Remove(link);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<TemuLink>> GetAllTemuLinksAsync()
        {
            return await _context.TemuLinks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TemuLink>> GetPublicTemuLinksAsync()
        {
            return await _context.TemuLinks
                .Where(t => t.IsPublic)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
