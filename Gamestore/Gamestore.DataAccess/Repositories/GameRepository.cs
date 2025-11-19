using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class GameRepository(GamestoreDbContext context) : IGameRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task CreateGameAsync(GameEntity entity)
    {
        await _context.Games.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<GameEntity?> GetGameByKeyAsync(string key)
    {
        var game = await _context.Games.FirstOrDefaultAsync(g => g.Key == key);
        return game;
    }
}
