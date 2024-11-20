using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMemeU.Data;

namespace IMemeU.Models;

public record RegisterViewModel
{
    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string UserName { get; init; }
    
    [Required]
    [StringLength(50, MinimumLength = 6)]
    public string Password { get; init; }
    public RegisterViewModel(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}