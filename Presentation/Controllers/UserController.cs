using Microsoft.AspNetCore.Mvc;
using UserService.Application.UseCase;
using UserService.Domain.Model;
using UserService.Presentation.DTOs;

namespace UserService.Presentation.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly RegisterUserUseCase _registerUserUseCase;

    public UserController(RegisterUserUseCase registerUserUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserRegisterResponse>> Register([FromBody] UserRegisterRequest request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            IdentityDocument = request.IdentityDocument,
            FullName = request.FullName,
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            Weight = request.Weight,
            Height = request.Height
        };

        var createdUser = await _registerUserUseCase.ExecuteAsync(user, cancellationToken);

        var response = new UserRegisterResponse(createdUser.Id, createdUser.Username, createdUser.Email, "Usuario registrado exitosamente");
        return StatusCode(StatusCodes.Status201Created, response);
    }
}