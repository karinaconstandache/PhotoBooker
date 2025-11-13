using PhotoBooker.Server.Models.Enums;

namespace PhotoBooker.Server.Models;

public class Shooting
{
    public int Id { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public ShootingStatus Status { get; set; } = ShootingStatus.Confirmed;
    
    // Foreign keys
    public string ClientId { get; set; } = string.Empty;
    public string PhotographerId { get; set; } = string.Empty;
    public int PackageId { get; set; }
    
    // Navigation properties
    public Client Client { get; set; } = null!;
    public Photographer Photographer { get; set; } = null!;
    public Package Package { get; set; } = null!;
}
