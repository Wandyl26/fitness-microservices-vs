using UserService.Domain.Model;

namespace UserService.Domain.Repository;

public interface IUserRepository
{
    Task<User> SaveAsync(User user, CancellationToken cancellationToken);
    Task<User?> FindByUsernameOrEmailAsync(string username, string email, CancellationToken cancellationToken);
}