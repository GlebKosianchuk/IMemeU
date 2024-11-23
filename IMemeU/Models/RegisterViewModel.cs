using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMemeU.Data;

namespace IMemeU.Models

{
    public record RegisterViewModel(
        [property: Required]
        [property: StringLength(50, MinimumLength = 3)]
        string UserName,

        [property: Required]
        [property: StringLength(50, MinimumLength = 6)]
        string Password
    );
}