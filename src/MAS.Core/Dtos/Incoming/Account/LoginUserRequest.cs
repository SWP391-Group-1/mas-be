using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Account;

public class LoginUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MaxLength(30, ErrorMessage = "Password must be less than 30 characters")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; }
}
