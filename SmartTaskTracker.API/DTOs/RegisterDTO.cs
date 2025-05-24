using System.ComponentModel.DataAnnotations;

namespace SmartTaskTracker.API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } 
        public string Role { get; set; } = "User";
    }
}
