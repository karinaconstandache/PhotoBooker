namespace PhotoBooker.Server.Models;

public class Portfolio
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Foreign key
    public string PhotographerId { get; set; } = string.Empty;
    
    // Navigation properties
    public Photographer Photographer { get; set; } = null!;
    public ICollection<PortfolioImage> Images { get; set; } = new List<PortfolioImage>();
}
