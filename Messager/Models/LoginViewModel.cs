using System.ComponentModel.DataAnnotations;

namespace Messager.Models;

public class LoginViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string Username { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 8)]
    public string Password { get; set; }
}