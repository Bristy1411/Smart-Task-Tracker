using System.ComponentModel.DataAnnotations;

namespace SmartTaskTracker.API.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
