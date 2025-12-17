using System.ComponentModel.DataAnnotations;

namespace PhotoBooker.Application.DTOs.Portfolios;

public class UpdatePortfolioDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
}
