namespace BHS.Domain.Photos;

public class InvalidPhotoIdException : Exception
{
    public InvalidPhotoIdException() { }

    public InvalidPhotoIdException(string? message) : base(message) { }

    public InvalidPhotoIdException(string? message, Exception? innerException) : base(message, innerException) { }
}
