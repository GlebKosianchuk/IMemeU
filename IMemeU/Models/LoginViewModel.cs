using System.ComponentModel.DataAnnotations;

namespace IMemeU.Models

{
    public class LoginViewModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; init; }
    
        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; init; }
    }
}