using Microsoft.EntityFrameworkCore;
using PhotoBooker.Application.Interfaces;
using PhotoBooker.Domain.Entities;
using PhotoBooker.Infrastructure.Data;

namespace PhotoBooker.Infrastructure.Repositories;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly AppDbContext _context;

    public PortfolioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Portfolio>> GetAllAsync()
    {
        return await _context.Portfolios
            .Include(p => p.Photographer)
            .ToListAsync();
    }

    public async Task<IEnumerable<Portfolio>> GetByPhotographerIdAsync(int photographerId)
    {
        return await _context.Portfolios
            .Where(p => p.PhotographerId == photographerId)
            .Include(p => p.Photographer)
            .OrderByDescending(p => p.CreatedDate)
            .ToListAsync();
    }

    public async Task<Portfolio?> GetByIdAsync(int id)
    {
        return await _context.Portfolios
            .Include(p => p.Photographer)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Portfolio portfolio)
    {
        await _context.Portfolios.AddAsync(portfolio);
    }

    public async Task UpdateAsync(Portfolio portfolio)
    {
        _context.Portfolios.Update(portfolio);
        await Task.CompletedTask;
    }

    public async Task DeleteAsync(int id)
    {
        var portfolio = await GetByIdAsync(id);
        if (portfolio != null)
        {
            _context.Portfolios.Remove(portfolio);
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Portfolios.AnyAsync(p => p.Id == id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
