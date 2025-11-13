using Microsoft.AspNetCore.Identity;
using PhotoBooker.Server.Models.Enums;

namespace PhotoBooker.Server.Models;

public class Photographer : ApplicationUser
{
    public string? Bio { get; set; }
    
    // Navigation properties
    public ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    public ICollection<Package> Packages { get; set; } = new List<Package>();
    public ICollection<Availability> Availabilities { get; set; } = new List<Availability>();
    public ICollection<BookingRequest> ReceivedBookingRequests { get; set; } = new List<BookingRequest>();
    public ICollection<Shooting> PhotographerShootings { get; set; } = new List<Shooting>();
    
    public Photographer()
    {
        Role = UserRole.Photographer;
    }
}
