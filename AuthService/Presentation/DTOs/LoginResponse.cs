namespace AuthService.Presentation.DTOs;

public record LoginResponse(string Token, string Username, string Message);