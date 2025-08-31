using TemuDB.API.Data;
using TemuDB.API.Models;

namespace TemuDB.API.Services
{
    public class TemuLinkService
    {
        private readonly JsonDataContext _dataContext;

        public TemuLinkService(JsonDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<TemuLink> GetTemuLinksByUser(string username)
        {
            var allLinks = _dataContext.GetTemuLinks();
            return allLinks.Where(t => t.Username == username).ToList();
        }

        public TemuLink? AddTemuLink(string username, string description, string link, bool isPublic = false)
        {
            var temuLink = new TemuLink
            {
                Username = username,
                Description = description,
                Link = link,
                IsPublic = isPublic,
                CreatedAt = DateTime.UtcNow
            };

            _dataContext.AddTemuLink(temuLink);
            return temuLink;
        }

        public TemuLink? UpdateTemuLink(int id, string username, string description, string link, bool isPublic)
        {
            var allLinks = _dataContext.GetTemuLinks();
            var existingLink = allLinks.FirstOrDefault(t => t.Id == id && t.Username == username);

            if (existingLink != null)
            {
                existingLink.Description = description;
                existingLink.Link = link;
                existingLink.IsPublic = isPublic;

                _dataContext.UpdateTemuLink(existingLink);
                return existingLink;
            }

            return null;
        }

        public bool DeleteTemuLink(int id, string username)
        {
            var allLinks = _dataContext.GetTemuLinks();
            var link = allLinks.FirstOrDefault(t => t.Id == id && t.Username == username);

            if (link != null)
            {
                _dataContext.DeleteTemuLink(id);
                return true;
            }

            return false;
        }

        public List<TemuLink> GetAllTemuLinks()
        {
            return _dataContext.GetTemuLinks();
        }

        public List<TemuLink> GetPublicTemuLinks()
        {
            var allLinks = _dataContext.GetTemuLinks();
            return allLinks.Where(t => t.IsPublic).ToList();
        }
    }
}
