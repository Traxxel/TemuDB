using System.Text.Json;
using TemuDB.API.Models;

namespace TemuDB.API.Data
{
    public class JsonDataContext
    {
        private readonly string _dataDirectory;
        private readonly string _usersFile;
        private readonly string _temuLinksFile;
        
        public JsonDataContext()
        {
            _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Json");
            _usersFile = Path.Combine(_dataDirectory, "users.json");
            _temuLinksFile = Path.Combine(_dataDirectory, "temulinks.json");
            
            // Erstelle Datenverzeichnis falls es nicht existiert
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
            
            // Initialisiere Dateien falls sie nicht existieren
            InitializeFiles();
        }
        
        private void InitializeFiles()
        {
            if (!File.Exists(_usersFile))
            {
                var adminUser = new User
                {
                    Id = 1,
                    Username = "adminstefanmeyer",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("JJvmr111"),
                    DisplayName = "Administrator",
                    IsActive = true,
                    IsAdmin = true,
                    CreatedAt = DateTime.UtcNow,
                    ActivatedAt = DateTime.UtcNow
                };
                
                var users = new List<User> { adminUser };
                SaveUsers(users);
            }
            
            if (!File.Exists(_temuLinksFile))
            {
                SaveTemuLinks(new List<TemuLink>());
            }
        }
        
        public List<User> GetUsers()
        {
            if (!File.Exists(_usersFile))
                return new List<User>();
                
            var json = File.ReadAllText(_usersFile);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }
        
        public void SaveUsers(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_usersFile, json);
        }
        
        public List<TemuLink> GetTemuLinks()
        {
            if (!File.Exists(_temuLinksFile))
                return new List<TemuLink>();
                
            var json = File.ReadAllText(_temuLinksFile);
            return JsonSerializer.Deserialize<List<TemuLink>>(json) ?? new List<TemuLink>();
        }
        
        public void SaveTemuLinks(List<TemuLink> temuLinks)
        {
            var json = JsonSerializer.Serialize(temuLinks, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_temuLinksFile, json);
        }
        
        public void AddUser(User user)
        {
            var users = GetUsers();
            user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            SaveUsers(users);
        }
        
        public void UpdateUser(User user)
        {
            var users = GetUsers();
            var existingUser = users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                var index = users.IndexOf(existingUser);
                users[index] = user;
                SaveUsers(users);
            }
        }
        
        public void AddTemuLink(TemuLink temuLink)
        {
            var temuLinks = GetTemuLinks();
            temuLink.Id = temuLinks.Count > 0 ? temuLinks.Max(t => t.Id) + 1 : 1;
            temuLinks.Add(temuLink);
            SaveTemuLinks(temuLinks);
        }
        
        public void DeleteTemuLink(int id)
        {
            var temuLinks = GetTemuLinks();
            var linkToDelete = temuLinks.FirstOrDefault(t => t.Id == id);
            if (linkToDelete != null)
            {
                temuLinks.Remove(linkToDelete);
                SaveTemuLinks(temuLinks);
            }
        }
    }
}
