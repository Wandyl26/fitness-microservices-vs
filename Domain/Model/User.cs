namespace UserService.Domain.Model;

public class User
{
    public long Id { get; set; }
    public string IdentityDocument { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public decimal Height { get; set; }
}