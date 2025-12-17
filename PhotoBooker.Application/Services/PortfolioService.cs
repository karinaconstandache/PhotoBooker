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
            Category = p.Category,
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
            Category = p.Category,
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
            Category = portfolio.Category,
            Description = portfolio.Description,
            CreatedDate = portfolio.CreatedDate,
            PhotographerId = portfolio.PhotographerId,
            PhotographerName = $"{portfolio.Photographer.FirstName} {portfolio.Photographer.LastName}"
        };
    }

    public async Task<PortfolioWithImagesDto?> GetPortfolioWithImagesByIdAsync(int id)
    {
        var portfolio = await _portfolioRepository.GetByIdWithImagesAsync(id);
        
        if (portfolio == null)
            return null;

        return new PortfolioWithImagesDto
        {
            Id = portfolio.Id,
            Title = portfolio.Title,
            Description = portfolio.Description,
            Category = portfolio.Category,
            CreatedDate = portfolio.CreatedDate,
            PhotographerId = portfolio.PhotographerId,
            PhotographerName = $"{portfolio.Photographer.FirstName} {portfolio.Photographer.LastName}",
            Images = portfolio.Images
                .OrderBy(i => i.DisplayOrder)
                .Select(i => new PortfolioImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    DisplayOrder = i.DisplayOrder
                })
                .ToList()
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
            Category = createDto.Category,
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
            Category = portfolio.Category,
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

        portfolio.Category = updateDto.Category;
        portfolio.Title = updateDto.Title;
        portfolio.Description = updateDto.Description;

        await _portfolioRepository.UpdateAsync(portfolio);
        await _portfolioRepository.SaveChangesAsync();

        return new PortfolioDto
        {
            Id = portfolio.Id,
            Title = portfolio.Title,
            Description = portfolio.Description,
            Category = portfolio.Category,
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

    public async Task<PortfolioImageDto> AddImageToPortfolioAsync(int portfolioId, int photographerId, string imageUrl, int displayOrder)
    {
        var portfolio = await _portfolioRepository.GetByIdAsync(portfolioId);
        
        if (portfolio == null)
        {
            throw new InvalidOperationException("Portfolio not found");
        }

        // Verify the portfolio belongs to the photographer
        if (portfolio.PhotographerId != photographerId)
        {
            throw new UnauthorizedAccessException("You don't have permission to add images to this portfolio");
        }

        var portfolioImage = new PortfolioImage
        {
            ImageUrl = imageUrl,
            DisplayOrder = displayOrder,
            PortfolioId = portfolioId
        };

        await _portfolioRepository.AddImageAsync(portfolioImage);
        await _portfolioRepository.SaveChangesAsync();

        return new PortfolioImageDto
        {
            Id = portfolioImage.Id,
            ImageUrl = portfolioImage.ImageUrl,
            DisplayOrder = portfolioImage.DisplayOrder
        };
    }

    public async Task<bool> DeleteImageFromPortfolioAsync(int imageId, int photographerId)
    {
        var image = await _portfolioRepository.GetImageByIdAsync(imageId);
        
        if (image == null)
            return false;

        // Verify the portfolio belongs to the photographer
        var portfolio = await _portfolioRepository.GetByIdAsync(image.PortfolioId);
        if (portfolio == null || portfolio.PhotographerId != photographerId)
        {
            throw new UnauthorizedAccessException("You don't have permission to delete this image");
        }

        await _portfolioRepository.DeleteImageAsync(imageId);
        await _portfolioRepository.SaveChangesAsync();

        return true;
    }
}
