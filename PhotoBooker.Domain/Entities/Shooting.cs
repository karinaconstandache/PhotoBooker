using PhotoBooker.Domain.Enums;

namespace PhotoBooker.Domain.Entities;

public class Shooting
{
    public int Id { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public ShootingStatus Status { get; set; } = ShootingStatus.Confirmed;
    
    // Foreign keys
    public int ClientId { get; set; }
    public int PhotographerId { get; set; }
    public int PackageId { get; set; }
    
    // Navigation properties
    public Client Client { get; set; } = null!;
    public Photographer Photographer { get; set; } = null!;
    public Package Package { get; set; } = null!;
}
