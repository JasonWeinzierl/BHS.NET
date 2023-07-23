namespace BHS.Infrastructure.Repositories.Auth0;

public class InvalidAuthorRequestException : Exception
{
    public InvalidAuthorRequestException(string message, Exception inner) : base(message, inner) { }
}
