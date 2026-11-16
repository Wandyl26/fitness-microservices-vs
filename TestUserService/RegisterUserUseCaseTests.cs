using Moq;
using UserService.Application.UseCase;
using UserService.Domain.Exception;
using UserService.Domain.Model;
using UserService.Domain.Repository;
using Xunit;

namespace UserService.Tests;

public class RegisterUserUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly RegisterUserUseCase _registerUserUseCase;

    public RegisterUserUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _registerUserUseCase = new RegisterUserUseCase(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_NewUser_ReturnsSavedUser()
    {
        // Arrange
        var inputUser = new User
        {
            IdentityDocument = "12345678",
            FullName = "Wandyl Diaz",
            Username = "wdiaz",
            Email = "wandyl@test.com",
            Password = "secret123",
            Weight = 75.5m,
            Height = 1.78m
        };

        _userRepositoryMock
            .Setup(r => r.FindByUsernameOrEmailAsync(inputUser.Username, inputUser.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        _userRepositoryMock
            .Setup(r => r.SaveAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User u, CancellationToken _) =>
            {
                u.Id = 1L;
                return u;
            });

        // Act
        var result = await _registerUserUseCase.ExecuteAsync(inputUser, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1L, result.Id);
        Assert.Equal("wdiaz", result.Username);
        _userRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ExistingUser_ThrowsUserAlreadyExistsException()
    {
        // Arrange
        var inputUser = new User
        {
            Username = "wdiaz",
            Email = "wandyl@test.com"
        };

        var existingUser = new User
        {
            Id = 1L,
            Username = "wdiaz",
            Email = "wandyl@test.com"
        };

        _userRepositoryMock
            .Setup(r => r.FindByUsernameOrEmailAsync(inputUser.Username, inputUser.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UserAlreadyExistsException>(() =>
            _registerUserUseCase.ExecuteAsync(inputUser, CancellationToken.None));

        Assert.Equal("El usuario o correo ya se encuentra registrado.", exception.Message);
        _userRepositoryMock.Verify(r => r.SaveAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}