namespace BHS.Domain.Leadership;

public class InvalidOfficeTitlesException : Exception
{
    public InvalidOfficeTitlesException() { }

    public InvalidOfficeTitlesException(string? message) : base(message) { }

    public InvalidOfficeTitlesException(string? message, Exception? innerException) : base(message, innerException) { }
}
