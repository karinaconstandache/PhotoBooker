namespace PhotoBooker.Domain.Exceptions;

public class PhotographerNotAvailableException : PhotoBookerException
{
    public string PhotographerId { get; }
    public DateTime RequestedDate { get; }

    public PhotographerNotAvailableException(string photographerId, DateTime requestedDate) 
        : base($"Photographer {photographerId} is not available on {requestedDate:yyyy-MM-dd}.")
    {
        PhotographerId = photographerId;
        RequestedDate = requestedDate;
    }

    public PhotographerNotAvailableException(string photographerId, DateTime requestedDate, string message) 
        : base(message)
    {
        PhotographerId = photographerId;
        RequestedDate = requestedDate;
    }
}
