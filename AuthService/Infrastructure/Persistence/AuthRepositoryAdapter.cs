using AuthService.Domain.Model;
using AuthService.Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Persistence;

public class AuthRepositoryAdapter : IAuthRepository
{
    private readonly AuthDbContext _context;

    public AuthRepositoryAdapter(AuthDbContext context)
    {
        _context = context;
    }

    public async Task<AuthUser?> FindByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .Where(u => u.Username == username)
            .Select(u => new AuthUserEntity
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                PasswordHash = u.PasswordHash
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) return null;

        return new AuthUser
        {
            Id = entity.Id,
            Username = entity.Username,
            Email = entity.Email,
            PasswordHash = entity.PasswordHash
        };
    }
}