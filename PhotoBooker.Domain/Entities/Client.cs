using PhotoBooker.Domain.Enums;

namespace PhotoBooker.Domain.Entities;

public class Client : User
{
    // Navigation properties
    public ICollection<BookingRequest> SentBookingRequests { get; set; } = new List<BookingRequest>();
    public ICollection<Shooting> ClientShootings { get; set; } = new List<Shooting>();

}
