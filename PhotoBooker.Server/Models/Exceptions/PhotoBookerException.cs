namespace PhotoBooker.Server.Models.Exceptions;

// Base exception class for PhotoBooker application (inheritance example)
public class PhotoBookerException : Exception
{
    public PhotoBookerException() : base()
    {
    }

    public PhotoBookerException(string message) : base(message)
    {
    }

    public PhotoBookerException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}
