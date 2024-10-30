using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Messager.Data
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
        
    }
}