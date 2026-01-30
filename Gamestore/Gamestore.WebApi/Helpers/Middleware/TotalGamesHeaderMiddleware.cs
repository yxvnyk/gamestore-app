using Gamestore.DataAccess.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Gamestore.WebApi.Helpers.Middleware;

public class TotalGamesHeaderMiddleware(IGameRepository gameSerivece, IMemoryCache cache) : IMiddleware
{
    private readonly IGameRepository _gameService = gameSerivece;
    private readonly IMemoryCache _cache = cache;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var cachedTotalGames = await GetTotalGamesCountAsync();

        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CustomHeaders.TotalGamesCount] = cachedTotalGames.ToString();
            return Task.CompletedTask;
        });
        await next(context);
    }

    public async Task<int> GetTotalGamesCountAsync()
    {
        return await _cache.GetOrCreateAsync(CustomHeaders.TotalGamesCount, async entry =>
        {
            // or SlidingExpiration
            entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
            return await _gameService.GetTotalGamesCountAsync();
        });
    }
}
