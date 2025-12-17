using PhotoBooker.Domain.Entities;

namespace PhotoBooker.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task<bool> UsernameExistsAsync(string username);
    Task<User> AddAsync(User user);
    Task SaveChangesAsync();
}
