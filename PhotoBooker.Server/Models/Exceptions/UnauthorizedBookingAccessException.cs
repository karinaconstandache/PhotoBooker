namespace PhotoBooker.Server.Models.Exceptions;

public class UnauthorizedBookingAccessException : PhotoBookerException
{
    public string UserId { get; }
    public int BookingId { get; }

    public UnauthorizedBookingAccessException(string userId, int bookingId) 
        : base($"User {userId} is not authorized to access booking {bookingId}.")
    {
        UserId = userId;
        BookingId = bookingId;
    }

    public UnauthorizedBookingAccessException(string userId, int bookingId, string message) 
        : base(message)
    {
        UserId = userId;
        BookingId = bookingId;
    }
}
