namespace PhotoBooker.Domain.Exceptions;

public class BookingNotFoundException : PhotoBookerException
{
    public int BookingId { get; }

    public BookingNotFoundException(int bookingId) 
        : base($"Booking with ID {bookingId} was not found.")
    {
        BookingId = bookingId;
    }

    public BookingNotFoundException(int bookingId, string message) 
        : base(message)
    {
        BookingId = bookingId;
    }
}
