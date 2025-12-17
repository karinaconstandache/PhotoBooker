namespace PhotoBooker.Domain.Entities;

using PhotoBooker.Domain.Enums;

public class Portfolio
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PortfolioCategory Category { get; set; } = PortfolioCategory.Other;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Foreign key
    public int PhotographerId { get; set; }
    
    // Navigation properties
    public Photographer Photographer { get; set; } = null!;
    public ICollection<PortfolioImage> Images { get; set; } = new List<PortfolioImage>();
}
