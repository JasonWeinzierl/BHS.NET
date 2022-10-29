namespace BHS.Domain.ContactUs;

public class InvalidContactRequestException : Exception
{
    public InvalidContactRequestException(string message) : base(message) { }

    public InvalidContactRequestException(string message, Exception inner) : base(message, inner) { }
}
