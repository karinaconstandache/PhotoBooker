using Microsoft.AspNetCore.Identity;
using PhotoBooker.Server.Models.Enums;

namespace PhotoBooker.Server.Models;

public class Client : ApplicationUser
{
    // Navigation properties
    public ICollection<BookingRequest> SentBookingRequests { get; set; } = new List<BookingRequest>();
    public ICollection<Shooting> ClientShootings { get; set; } = new List<Shooting>();
    
    public Client()
    {
        Role = UserRole.Client;
    }
}
