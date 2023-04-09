namespace ContactService.Exceptions;

public class UserNotFound : HttpException
{
    public UserNotFound() : base(404, "User not found.")
    {
    }
}