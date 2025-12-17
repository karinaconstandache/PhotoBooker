namespace PhotoBooker.Application.DTOs.Portfolios;

public class PortfolioImageDto
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
}
