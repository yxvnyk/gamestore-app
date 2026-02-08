using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Entities;
using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.DataAccess.Repositories;

public class CommentRepository(GamestoreDbContext context) : ICommentRepository
{
    private readonly GamestoreDbContext _context = context;

    public async Task CreateAsync(Comment entity)
    {
        await _context.Comments.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsExistAsync(Guid id)
    {
        var exist = await _context.Comments.AnyAsync(g => g.Id == id);
        return exist;
    }

    public async Task UpdateAsync(Comment entity)
    {
        _context.Comments.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exist = await _context.Comments.FindAsync(id);
        if (exist != null)
        {
            _ = _context.Comments.Remove(exist);
            _ = await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<IEnumerable<Comment>?> GetByGameKeyAsync(string key)
    {
        var comments = await _context.Comments
            .AsNoTracking()
            .Where(p => p.Game.Key == key)
            .ToListAsync();
        return comments;
    }
}