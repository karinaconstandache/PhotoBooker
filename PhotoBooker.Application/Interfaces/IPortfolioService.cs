using PhotoBooker.Application.DTOs.Portfolios;

namespace PhotoBooker.Application.Interfaces;

public interface IPortfolioService
{
    Task<IEnumerable<PortfolioDto>> GetAllPortfoliosAsync();
    Task<IEnumerable<PortfolioDto>> GetPhotographerPortfoliosAsync(int photographerId);
    Task<PortfolioDto?> GetPortfolioByIdAsync(int id);
    Task<PortfolioDto> CreatePortfolioAsync(int photographerId, CreatePortfolioDto createDto);
    Task<PortfolioDto?> UpdatePortfolioAsync(int portfolioId, int photographerId, UpdatePortfolioDto updateDto);
    Task<bool> DeletePortfolioAsync(int portfolioId, int photographerId);
}
