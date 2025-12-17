using PhotoBooker.Domain.Enums;

namespace PhotoBooker.Domain.Entities;

public class BookingRequest
{
    public int Id { get; set; }
    public DateTime RequestedDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public string? Message { get; set; }
    
    // Foreign keys
    public int ClientId { get; set; }
    public int PhotographerId { get; set; }
    public int PackageId { get; set; }
    
    // Navigation properties
    public Client Client { get; set; } = null!;
    public Photographer Photographer { get; set; } = null!;
    public Package Package { get; set; } = null!;
}
