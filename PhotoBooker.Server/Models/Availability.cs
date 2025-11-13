namespace PhotoBooker.Server.Models;

public class Availability
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsAvailable { get; set; } = true;
    
    // Foreign key
    public string PhotographerId { get; set; } = string.Empty;
    
    // Navigation property
    public Photographer Photographer { get; set; } = null!;
}
