using System.ComponentModel.DataAnnotations;

namespace IMemeU.Data
{
    public class User
    {
        public int Id { get; init; }
        
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string UserName { get; init; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; }
    }
}