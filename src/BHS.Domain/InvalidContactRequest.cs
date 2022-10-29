namespace BHS.Domain;

public class InvalidContactRequest : Exception
{
    public InvalidContactRequest(string message) : base(message) { }

    public InvalidContactRequest(string message, Exception inner) : base(message, inner) { }
}
