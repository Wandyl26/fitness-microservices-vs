using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Infrastructure.Persistence;

[Table("users")]
public class UserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [Column("identity_document")]
    public string IdentityDocument { get; set; } = string.Empty;

    [Required]
    [Column("full_name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [Column("weight")]
    public decimal Weight { get; set; }

    [Required]
    [Column("height")]
    public decimal Height { get; set; }
}