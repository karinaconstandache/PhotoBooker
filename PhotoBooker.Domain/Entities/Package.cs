namespace PhotoBooker.Domain.Entities;

public class Package
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int DurationHours { get; set; }
    public int EditedPhotosCount { get; set; }
    public string Description { get; set; } = string.Empty;
    
    // Foreign keys
    public int PhotographerId { get; set; }
    public int ShootingCategoryId { get; set; }
    
    // Navigation properties
    public Photographer Photographer { get; set; } = null!;
    public ShootingCategory ShootingCategory { get; set; } = null!;
    public ICollection<BookingRequest> BookingRequests { get; set; } = new List<BookingRequest>();
    public ICollection<Shooting> Shootings { get; set; } = new List<Shooting>();
}
