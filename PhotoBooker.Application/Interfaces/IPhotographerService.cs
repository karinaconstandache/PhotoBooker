using PhotoBooker.Application.DTOs.Photographers;

namespace PhotoBooker.Application.Interfaces;

public interface IPhotographerService
{
    Task<IEnumerable<PhotographerDto>> GetAllPhotographersAsync();
    Task<PhotographerDto?> GetPhotographerByIdAsync(int id);
    Task<PhotographerDto?> UpdatePhotographerAsync(int userId, UpdatePhotographerDto updateDto);
}
