namespace PhotoBooker.Domain.Exceptions;

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
