using PhotoBooker.Domain.Entities;

namespace PhotoBooker.Application.Interfaces;

public interface IPortfolioRepository
{
    Task<IEnumerable<Portfolio>> GetAllAsync();
    Task<IEnumerable<Portfolio>> GetByPhotographerIdAsync(int photographerId);
    Task<Portfolio?> GetByIdAsync(int id);
    Task<Portfolio?> GetByIdWithImagesAsync(int id);
    Task AddAsync(Portfolio portfolio);
    Task UpdateAsync(Portfolio portfolio);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task SaveChangesAsync();
    Task AddImageAsync(PortfolioImage image);
    Task<PortfolioImage?> GetImageByIdAsync(int imageId);
    Task DeleteImageAsync(int imageId);
}
