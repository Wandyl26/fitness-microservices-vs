using AuthService.Domain.Exception;
using AuthService.Domain.Repository;
using AuthService.Presentation.DTOs;

namespace AuthService.Application.UseCase;

public class LoginUseCase
{
    private readonly IAuthRepository _authRepository;

    public LoginUseCase(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<LoginResponse> AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
    {
        var user = await _authRepository.FindByUsernameAsync(username, cancellationToken);
        if (user == null || user.PasswordHash != password)
        {
            throw new InvalidCredentialsException("Credenciales inválidas");
        }

        return new LoginResponse("mock-jwt-token-dotnet-2026", user.Username, "Autenticación exitosa");
    }
}