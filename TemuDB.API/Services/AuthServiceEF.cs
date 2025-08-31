using Microsoft.EntityFrameworkCore;
using TemuDB.API.Data;
using TemuDB.API.Models;

namespace TemuDB.API.Services
{
    public class AuthServiceEF
    {
        private readonly ApplicationDbContext _context;

        public AuthServiceEF(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        public async Task<bool> RegisterUserAsync(RegisterRequest request)
        {
            // Prüfe ob Benutzername bereits existiert
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
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

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                user.IsActive = true;
                user.ActivatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<User>> GetInactiveUsersAsync()
        {
            return await _context.Users
                .Where(u => !u.IsActive)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
