namespace PhotoBooker.Application.DTOs.Portfolios;

public class PortfolioDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public int PhotographerId { get; set; }
    public string PhotographerName { get; set; } = string.Empty;
}
