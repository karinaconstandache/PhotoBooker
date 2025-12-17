namespace PhotoBooker.Application.DTOs.Portfolios;

using PhotoBooker.Domain.Enums;

public class PortfolioWithImagesDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PortfolioCategory Category { get; set; }
    public DateTime CreatedDate { get; set; }
    public int PhotographerId { get; set; }
    public string PhotographerName { get; set; } = string.Empty;
    public List<PortfolioImageDto> Images { get; set; } = new();
}
