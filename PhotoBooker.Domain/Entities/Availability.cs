namespace PhotoBooker.Domain.Entities;

public class Availability
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsAvailable { get; set; } = true;
    
    // Foreign key
    public int PhotographerId { get; set; }
    
    // Navigation property
    public Photographer Photographer { get; set; } = null!;
}
