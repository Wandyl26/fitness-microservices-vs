using AuthService.Domain.Model;

namespace AuthService.Domain.Repository;

public interface IAuthRepository
{
    Task<AuthUser?> FindByUsernameAsync(string username, CancellationToken cancellationToken);
}