namespace UserService.Domain.Exception;

public class UserAlreadyExistsException : System.Exception
{
    public UserAlreadyExistsException(string message) : base(message) { }
}