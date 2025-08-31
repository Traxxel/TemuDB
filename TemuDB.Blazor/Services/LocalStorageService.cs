using Microsoft.JSInterop;
using System.Text.Json;

namespace TemuDB.Blazor.Services
{
    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;
        
        public LocalStorageService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        
        public async Task<T?> GetItemAsync<T>(string key)
        {
            try
            {
                var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
                if (string.IsNullOrEmpty(json))
                    return default;
                    
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (InvalidOperationException)
            {
                // JavaScript ist während Prerendering nicht verfügbar
                return default;
            }
        }
        
        public async Task SetItemAsync<T>(string key, T value)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, json);
            }
            catch (InvalidOperationException)
            {
                // JavaScript ist während Prerendering nicht verfügbar
            }
        }
        
        public async Task RemoveItemAsync(string key)
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            }
            catch (InvalidOperationException)
            {
                // JavaScript ist während Prerendering nicht verfügbar
            }
        }
    }
}
