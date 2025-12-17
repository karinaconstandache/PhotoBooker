using PhotoBooker.Domain.Entities;

namespace PhotoBooker.Application.Interfaces;

public interface IPhotographerRepository
{
    Task<IEnumerable<Photographer>> GetAllAsync();
    Task<Photographer?> GetByIdAsync(int id);
    Task<Photographer?> GetByUserIdAsync(int userId);
    Task AddAsync(Photographer photographer);
    Task UpdateAsync(Photographer photographer);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task SaveChangesAsync();
}
