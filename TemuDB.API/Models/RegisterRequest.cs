using System.ComponentModel.DataAnnotations;

namespace TemuDB.API.Models
{
    public class RegisterRequest
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; } = string.Empty;
    }
}
