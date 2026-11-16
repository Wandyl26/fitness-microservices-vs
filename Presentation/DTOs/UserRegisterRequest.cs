using System.ComponentModel.DataAnnotations;

namespace UserService.Presentation.DTOs;

public class UserRegisterRequest
{
    [Required(ErrorMessage = "El documento es obligatorio")]
    public string IdentityDocument { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required, Range(0.1, 500)]
    public decimal Weight { get; set; }

    [Required, Range(0.1, 3)]
    public decimal Height { get; set; }
}