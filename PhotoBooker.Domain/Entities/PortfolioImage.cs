namespace PhotoBooker.Domain.Entities;

public class PortfolioImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    
    // Foreign key
    public int PortfolioId { get; set; }
    
    // Navigation property
    public Portfolio Portfolio { get; set; } = null!;
}
