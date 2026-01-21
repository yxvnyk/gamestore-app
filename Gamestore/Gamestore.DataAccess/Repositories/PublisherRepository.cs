using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class PublisherRepository(GamestoreDbContext context) : IPublisherRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task CreatePublisherAsync(Publisher entity)
    {
        await _context.Publishers.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePublisherAsync(Publisher entity)
    {
        _context.Publishers.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Publisher?> GetPublisherByGameKeyAsync(string key)
    {
        var publishers = await _context.Publishers
            .AsNoTracking()
            .Where(p => p.Games.Any(g => g.Key == key))
            .FirstOrDefaultAsync();
        return publishers;
    }

    public async Task<Publisher?> GetPublisherByCompanyNameAsync(string companyName)
    {
        var publisher = await _context.Publishers
            .FirstOrDefaultAsync(p => p.CompanyName == companyName);
        return publisher;
    }

    public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
    {
        var publishers = await _context.Publishers.ToListAsync();
        return publishers;
    }

    public async Task<bool> PublisherExistAsync(Guid id)
    {
        var exist = await _context.Publishers.AnyAsync(p => p.Id == id);
        return exist;
    }

    public async Task<bool> PublisherCompanyNameExistAsync(string companyName)
    {
        var exist = await _context.Publishers.AnyAsync(p => p.CompanyName == companyName);
        return exist;
    }
}