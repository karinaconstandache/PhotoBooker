using Microsoft.EntityFrameworkCore;
using PhotoBooker.Application.Interfaces;
using PhotoBooker.Domain.Entities;
using PhotoBooker.Infrastructure.Data;

namespace PhotoBooker.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        return Task.FromResult(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
