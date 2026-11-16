using UserService.Domain.Exception;
using UserService.Domain.Model;
using UserService.Domain.Repository;

namespace UserService.Application.UseCase;

public class RegisterUserUseCase
{
    private readonly IUserRepository _userRepository;

    public RegisterUserUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> ExecuteAsync(User user, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.FindByUsernameOrEmailAsync(user.Username, user.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new UserAlreadyExistsException("El usuario o correo ya se encuentra registrado.");
        }

        return await _userRepository.SaveAsync(user, cancellationToken);
    }
}