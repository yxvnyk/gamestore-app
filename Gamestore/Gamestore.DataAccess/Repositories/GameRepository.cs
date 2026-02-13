using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Extensions;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Models;
using Gamestore.Domain.Models.DTO.Game;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class GameRepository(GamestoreDbContext context) : IGameRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task CreateGameAsync(Game entity)
    {
        await _context.Games.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<Game?> GetGameByKeyAsync(string key)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Key == key);
        return game;
    }

    public async Task<Guid?> GetIdByKeyAsync(string key)
    {
        var gameId = await _context.Games
        .Where(g => g.Key == key)
        .Select(g => (Guid?)g.Id)
        .FirstOrDefaultAsync();

        return gameId;
    }

    public async Task<bool> GameKeyExistAsync(string key)
    {
        var exist = await _context.Games.AnyAsync(g => g.Key == key);
        return exist;
    }

    public async Task<ICollection<Game>> GetGamesByPlatformAsync(Guid id)
    {
        return await _context.Games
            .Include(g => g.GamePlatforms)
            .ThenInclude(gp => gp.Platform)
            .Where(g => g.GamePlatforms.Any(gp => gp.PlatformId == id)).ToListAsync();
    }

    public async Task<ICollection<Game>> GetGamesByGenreAsync(Guid id)
    {
        return await _context.Games
            .Include(g => g.GameGenres)
            .ThenInclude(gp => gp.Genre)
            .Where(g => g.GameGenres.Any(gp => gp.GenreId == id)).ToListAsync();
    }

    public async Task<ICollection<Game>> GetGamesByCompanyNameAsync(string companyName)
    {
        return await _context.Games
            .Include(g => g.Publisher)
            .Where(g => g.Publisher.CompanyName == companyName).ToListAsync();
    }

    public async Task<PagedList<Game>> GetAllGamesAsync(GetGamesRequest request)
    {
        var games = _context.Games.AsNoTracking();

        var count = await games.CountAsync();

        var items = await games
            .ApplyGameNameFiltration(request.Name)
            .ApplyPriceRangeFiltration(request.MinPrice, request.MaxPrice)
            .ApplyPublishDateFiltration(request.DatePublishing)
            .ApplySorting(request.Sort)
            .ApplyPaging(request.Page, request.ActualPageSize)
            .ToListAsync();

        return new PagedList<Game>()
        {
            Items = items,
            TotalCount = count,
        };
    }

    public async Task<Guid?> GetGameIdByKeyAsync(string key)
    {
        return await _context.Games
            .Where(g => g.Key == key)
            .Select(g => g.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<Game?> GetGameByIdAsync(Guid id)
    {
        var game = await _context.Games.FindAsync(id);
        return game;
    }

    public async Task<Game?> GetGameWithJoinsAsync(Guid id)
    {
        var game = await _context.Games
            .Include(g => g.GameGenres)
            .Include(p => p.GamePlatforms)
            .FirstOrDefaultAsync(g => g.Id == id);
        return game;
    }

    public async Task UpdateGameAsync(Game entity)
    {
        _context.Games.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteByKeyAsync(string key)
    {
        var exist = await _context.Games.FirstOrDefaultAsync(g => g.Key == key);
        if (exist != null)
        {
            _ = _context.Games.Remove(exist);
            _ = await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<int> GetUnitsInStockAsync(Guid gameId)
    {
        return await _context.Games
            .Where(g => g.Id == gameId)
            .Select(g => g.UnitInStock)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GetTotalGamesCountAsync()
    {
        return await _context.Games.CountAsync();
    }
}
