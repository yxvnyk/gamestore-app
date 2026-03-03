using Gamestore.DataAccess.Northwind.Context;

namespace Gamestore.WebApi.Extensions;

public static class ApplicaitonBuilderExtension
{
    public static async Task InitializeNorthwindDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<NothwindDbInitializer>();
        await initializer.InitializeGameKeyAync();
    }
}
