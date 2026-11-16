using Microsoft.EntityFrameworkCore;
using UserService.Domain.Model;
using UserService.Domain.Repository;

namespace UserService.Infrastructure.Persistence;

public class UserRepositoryAdapter : IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepositoryAdapter(UserDbContext context)
    {
        _context = context;
    }

    public async Task<User> SaveAsync(User user, CancellationToken cancellationToken)
    {
        var entity = new UserEntity
        {
            IdentityDocument = user.IdentityDocument,
            FullName = user.FullName,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.Password,
            Weight = user.Weight,
            Height = user.Height
        };

        _context.Users.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        user.Id = entity.Id;
        return user;
    }

    public async Task<User?> FindByUsernameOrEmailAsync(string username, string email, CancellationToken cancellationToken)
    {
        var entity = await _context.Users
            .AsNoTracking()
            .Where(u => u.Username == username || u.Email == email)
            .Select(u => new UserEntity
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null) return null;

        return new User
        {
            Id = entity.Id,
            Username = entity.Username,
            Email = entity.Email
        };
    }
}