using PhotoBooker.Application.DTOs.Photographers;
using PhotoBooker.Application.Interfaces;

namespace PhotoBooker.Application.Services;

public class PhotographerService : IPhotographerService
{
    private readonly IPhotographerRepository _photographerRepository;

    public PhotographerService(IPhotographerRepository photographerRepository)
    {
        _photographerRepository = photographerRepository;
    }

    public async Task<IEnumerable<PhotographerDto>> GetAllPhotographersAsync()
    {
        var photographers = await _photographerRepository.GetAllAsync();
        
        return photographers.Select(p => new PhotographerDto
        {
            Id = p.Id,
            Username = p.Username,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Bio = p.Bio,
            CreatedAt = p.CreatedAt
        });
    }

    public async Task<PhotographerDto?> GetPhotographerByIdAsync(int id)
    {
        var photographer = await _photographerRepository.GetByIdAsync(id);
        
        if (photographer == null)
            return null;

        return new PhotographerDto
        {
            Id = photographer.Id,
            Username = photographer.Username,
            FirstName = photographer.FirstName,
            LastName = photographer.LastName,
            Bio = photographer.Bio,
            CreatedAt = photographer.CreatedAt
        };
    }

    public async Task<PhotographerDto?> UpdatePhotographerAsync(int userId, UpdatePhotographerDto updateDto)
    {
        var photographer = await _photographerRepository.GetByUserIdAsync(userId);
        
        if (photographer == null)
            return null;

        photographer.Bio = updateDto.Bio;
        
        await _photographerRepository.UpdateAsync(photographer);
        await _photographerRepository.SaveChangesAsync();

        return new PhotographerDto
        {
            Id = photographer.Id,
            Username = photographer.Username,
            FirstName = photographer.FirstName,
            LastName = photographer.LastName,
            Bio = photographer.Bio,
            CreatedAt = photographer.CreatedAt
        };
    }
}
