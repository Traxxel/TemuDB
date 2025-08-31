using TemuDB.API.Data;
using TemuDB.API.Models;

namespace TemuDB.API.Services
{
    public class AuthService
    {
        private readonly JsonDataContext _dataContext;
        
        public AuthService(JsonDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        
        public User? AuthenticateUser(string username, string password)
        {
            var users = _dataContext.GetUsers();
            var user = users.FirstOrDefault(u => u.Username == username && u.IsActive);
            
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }
            
            return null;
        }
        
        public bool RegisterUser(RegisterRequest request)
        {
            var users = _dataContext.GetUsers();
            
            // Prüfe ob Benutzername bereits existiert
            if (users.Any(u => u.Username == request.Username))
            {
                return false;
            }
            
            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                DisplayName = request.DisplayName,
                IsActive = false, // Muss vom Admin freigeschaltet werden
                IsAdmin = false,
                CreatedAt = DateTime.UtcNow
            };
            
            _dataContext.AddUser(newUser);
            return true;
        }
        
        public bool ActivateUser(int userId)
        {
            var users = _dataContext.GetUsers();
            var user = users.FirstOrDefault(u => u.Id == userId);
            
            if (user != null)
            {
                user.IsActive = true;
                user.ActivatedAt = DateTime.UtcNow;
                _dataContext.UpdateUser(user);
                return true;
            }
            
            return false;
        }
        
        public List<User> GetInactiveUsers()
        {
            var users = _dataContext.GetUsers();
            return users.Where(u => !u.IsActive).ToList();
        }
        
        public List<User> GetAllUsers()
        {
            return _dataContext.GetUsers();
        }
    }
}
