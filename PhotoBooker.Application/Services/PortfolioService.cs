using PhotoBooker.Application.DTOs.Portfolios;
using PhotoBooker.Application.Interfaces;
using PhotoBooker.Domain.Entities;

namespace PhotoBooker.Application.Services;

public class PortfolioService : IPortfolioService
{
    private readonly IPortfolioRepository _portfolioRepository;
    private readonly IPhotographerRepository _photographerRepository;

    public PortfolioService(
        IPortfolioRepository portfolioRepository,
        IPhotographerRepository photographerRepository)
    {
        _portfolioRepository = portfolioRepository;
        _photographerRepository = photographerRepository;
    }

    public async Task<IEnumerable<PortfolioDto>> GetAllPortfoliosAsync()
    {
        var portfolios = await _portfolioRepository.GetAllAsync();
        
        return portfolios.Select(p => new PortfolioDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            CreatedDate = p.CreatedDate,
            PhotographerId = p.PhotographerId,
            PhotographerName = $"{p.Photographer.FirstName} {p.Photographer.LastName}"
        });
    }

    public async Task<IEnumerable<PortfolioDto>> GetPhotographerPortfoliosAsync(int photographerId)
    {
        var portfolios = await _portfolioRepository.GetByPhotographerIdAsync(photographerId);
        
        return portfolios.Select(p => new PortfolioDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            CreatedDate = p.CreatedDate,
            PhotographerId = p.PhotographerId,
            PhotographerName = $"{p.Photographer.FirstName} {p.Photographer.LastName}"
        });
    }

    public async Task<PortfolioDto?> GetPortfolioByIdAsync(int id)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(id);
        
        if (portfolio == null)
            return null;

        return new PortfolioDto
        {
            Id = portfolio.Id,
            Title = portfolio.Title,
            Description = portfolio.Description,
            CreatedDate = portfolio.CreatedDate,
            PhotographerId = portfolio.PhotographerId,
            PhotographerName = $"{portfolio.Photographer.FirstName} {portfolio.Photographer.LastName}"
        };
    }

    public async Task<PortfolioDto> CreatePortfolioAsync(int photographerId, CreatePortfolioDto createDto)
    {
        // Verify photographer exists
        if (!await _photographerRepository.ExistsAsync(photographerId))
        {
            throw new InvalidOperationException("Photographer not found");
        }

        var portfolio = new Portfolio
        {
            Title = createDto.Title,
            Description = createDto.Description,
            PhotographerId = photographerId,
            CreatedDate = DateTime.UtcNow
        };

        await _portfolioRepository.AddAsync(portfolio);
        await _portfolioRepository.SaveChangesAsync();

        var photographer = await _photographerRepository.GetByIdAsync(photographerId);
        
        return new PortfolioDto
        {
            Id = portfolio.Id,
            Title = portfolio.Title,
            Description = portfolio.Description,
            CreatedDate = portfolio.CreatedDate,
            PhotographerId = portfolio.PhotographerId,
            PhotographerName = photographer != null 
                ? $"{photographer.FirstName} {photographer.LastName}" 
                : string.Empty
        };
    }

    public async Task<PortfolioDto?> UpdatePortfolioAsync(int portfolioId, int photographerId, UpdatePortfolioDto updateDto)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(portfolioId);
        
        if (portfolio == null)
            return null;

        // Verify the portfolio belongs to the photographer
        if (portfolio.PhotographerId != photographerId)
        {
            throw new UnauthorizedAccessException("You don't have permission to update this portfolio");
        }

        portfolio.Title = updateDto.Title;
        portfolio.Description = updateDto.Description;

        await _portfolioRepository.UpdateAsync(portfolio);
        await _portfolioRepository.SaveChangesAsync();

        return new PortfolioDto
        {
            Id = portfolio.Id,
            Title = portfolio.Title,
            Description = portfolio.Description,
            CreatedDate = portfolio.CreatedDate,
            PhotographerId = portfolio.PhotographerId,
            PhotographerName = $"{portfolio.Photographer.FirstName} {portfolio.Photographer.LastName}"
        };
    }

    public async Task<bool> DeletePortfolioAsync(int portfolioId, int photographerId)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(portfolioId);
        
        if (portfolio == null)
            return false;

        // Verify the portfolio belongs to the photographer
        if (portfolio.PhotographerId != photographerId)
        {
            throw new UnauthorizedAccessException("You don't have permission to delete this portfolio");
        }

        await _portfolioRepository.DeleteAsync(portfolioId);
        await _portfolioRepository.SaveChangesAsync();

        return true;
    }
}
