using System.ComponentModel.DataAnnotations;

namespace TemuDB.API.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; } = string.Empty;
        
        public bool IsActive { get; set; } = false;
        
        public bool IsAdmin { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? ActivatedAt { get; set; }
    }
}
