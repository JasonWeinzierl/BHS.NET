namespace BHS.Domain.Banners;

public class InvalidBannerIdException : Exception
{
    public InvalidBannerIdException() { }

    public InvalidBannerIdException(string? message) : base(message) { }

    public InvalidBannerIdException(string? message, Exception? innerException) : base(message, innerException) { }
}
