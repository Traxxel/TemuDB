using System.Text.Json;

namespace TemuDB.Blazor.Services
{
    public class AuthService
    {
        private readonly ApiService _apiService;
        private readonly ILocalStorageService _localStorage;
        
        public AuthService(ApiService apiService, ILocalStorageService localStorage)
        {
            _apiService = apiService;
            _localStorage = localStorage;
        }
        
        public event Action? OnAuthenticationStateChanged;
        
        public async Task<LoginResult?> LoginAsync(string username, string password)
        {
            var request = new { username, password };
            var result = await _apiService.PostAsync<LoginResult>("auth/login", request);
            
            if (result?.Success == true)
            {
                await _localStorage.SetItemAsync("currentUser", result.User);
                OnAuthenticationStateChanged?.Invoke();
            }
            
            return result;
        }
        
        public async Task<bool> RegisterAsync(string username, string password, string displayName)
        {
            var request = new { username, password, displayName };
            var result = await _apiService.PostAsync<RegisterResult>("auth/register", request);
            return result?.Success == true;
        }
        
        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("currentUser");
            OnAuthenticationStateChanged?.Invoke();
        }
        
        public async Task<User?> GetCurrentUserAsync()
        {
            return await _localStorage.GetItemAsync<User>("currentUser");
        }
        
        public async Task<bool> IsAuthenticatedAsync()
        {
            var user = await GetCurrentUserAsync();
            return user != null;
        }
    }
    
    public class LoginResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public User? User { get; set; }
    }
    
    public class RegisterResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
    
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
    }
}
