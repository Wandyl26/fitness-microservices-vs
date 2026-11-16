using AuthService.Application.UseCase;
using AuthService.Domain.Exception;
using AuthService.Domain.Model;
using AuthService.Domain.Repository;
using Moq;
using Xunit;

namespace AuthService.Tests;

public class LoginUseCaseTests
{
    private readonly Mock<IAuthRepository> _authRepositoryMock;
    private readonly LoginUseCase _loginUseCase;

    public LoginUseCaseTests()
    {
        _authRepositoryMock = new Mock<IAuthRepository>();
        _loginUseCase = new LoginUseCase(_authRepositoryMock.Object);
    }

    [Fact]
    public async Task AuthenticateAsync_ValidCredentials_ReturnsLoginResponse()
    {
        // Arrange
        var username = "wdiaz";
        var password = "secret123";
        var mockUser = new AuthUser
        {
            Id = 1L,
            Username = username,
            Email = "wdiaz@test.com",
            PasswordHash = password
        };

        _authRepositoryMock
            .Setup(r => r.FindByUsernameAsync(username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockUser);

        // Act
        var response = await _loginUseCase.AuthenticateAsync(username, password, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(username, response.Username);
        Assert.False(string.IsNullOrEmpty(response.Token));
        Assert.Equal("Autenticación exitosa", response.Message);
        _authRepositoryMock.Verify(r => r.FindByUsernameAsync(username, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AuthenticateAsync_UserNotFound_ThrowsInvalidCredentialsException()
    {
        // Arrange
        _authRepositoryMock
            .Setup(r => r.FindByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AuthUser?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _loginUseCase.AuthenticateAsync("usuario_inexistente", "secret123", CancellationToken.None));

        Assert.Equal("Credenciales inválidas", exception.Message);
    }

    [Fact]
    public async Task AuthenticateAsync_WrongPassword_ThrowsInvalidCredentialsException()
    {
        // Arrange
        var username = "wdiaz";
        var mockUser = new AuthUser
        {
            Id = 1L,
            Username = username,
            PasswordHash = "clave_correcta"
        };

        _authRepositoryMock
            .Setup(r => r.FindByUsernameAsync(username, It.IsAny<CancellationToken>()))
            .ReturnsAsync(mockUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _loginUseCase.AuthenticateAsync(username, "clave_incorrecta", CancellationToken.None));

        Assert.Equal("Credenciales inválidas", exception.Message);
    }
}