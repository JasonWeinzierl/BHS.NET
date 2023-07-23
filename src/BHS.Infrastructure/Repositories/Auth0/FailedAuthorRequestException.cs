namespace BHS.Infrastructure.Repositories.Auth0;

public class FailedAuthorRequestException : Exception
{
    public FailedAuthorRequestException(string message) : base(message) { }

    public FailedAuthorRequestException(string message, Exception inner) : base(message, inner) { }
}
