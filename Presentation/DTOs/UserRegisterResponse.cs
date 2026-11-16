namespace UserService.Presentation.DTOs;

public record UserRegisterResponse(long Id, string Username, string Email, string Message);