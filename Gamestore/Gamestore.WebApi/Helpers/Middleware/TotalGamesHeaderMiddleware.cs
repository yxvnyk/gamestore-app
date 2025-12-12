using Gamestore.DataAccess.Repositories.Interfaces;

namespace Gamestore.WebApi.Helpers.Middleware;

public class TotalGamesHeaderMiddleware(IGameService gameSerivece) : IMiddleware
{
    private readonly IGameService _gameService = gameSerivece;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Response.OnStarting(async () =>
        {
            var totalGames = await _gameService.GetTotalGamesCountAsync();
            context.Response.Headers[CustomHeaders.TotalGamesCount] = totalGames.ToString();
        });
        await next(context);
    }
}
