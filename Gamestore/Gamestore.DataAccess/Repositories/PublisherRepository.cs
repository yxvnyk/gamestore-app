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