using Microsoft.EntityFrameworkCore;
using PhotoBooker.Application.Interfaces;
using PhotoBooker.Domain.Entities;
using PhotoBooker.Infrastructure.Data;

namespace PhotoBooker.Infrastructure.Repositories;

public class PhotographerRepository : IPhotographerRepository
{
    private readonly AppDbContext _context;

    public PhotographerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Photographer>> GetAllAsync()
    {
        return await _context.Set<Photographer>()
            .Include(p => p.Portfolios)
            .ToListAsync();
    }

    public async Task<Photographer?> GetByIdAsync(int id)
    {
        return await _context.Set<Photographer>()
            .Include(p => p.Portfolios)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Photographer?> GetByUserIdAsync(int userId)
    {
        return await _context.Set<Photographer>()
            .Include(p => p.Portfolios)
            .FirstOrDefaultAsync(p => p.Id == userId);
    }

    public async Task AddAsync(Photographer photographer)
    {
        await _context.Set<Photographer>().AddAsync(photographer);
    }

    public async Task UpdateAsync(Photographer photographer)
    {
        _context.Set<Photographer>().Update(photographer);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var photographer = await GetByIdAsync(id);
        if (photographer != null)
        {
            _context.Set<Photographer>().Remove(photographer);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Set<Photographer>().AnyAsync(p => p.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
