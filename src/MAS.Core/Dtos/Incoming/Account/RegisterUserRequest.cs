using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Account;

public class RegisterUserRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [MaxLength(30, ErrorMessage = "Password must be less than 30 characters")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password",
        ErrorMessage = "Password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(1000)]
    public string Introduce { get; set; }
}
