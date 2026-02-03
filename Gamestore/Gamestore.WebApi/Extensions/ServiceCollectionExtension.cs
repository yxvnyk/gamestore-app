using Gamestore.Application.Helpers;
using GameStore.Application.Helpers;
using Gamestore.Application.Helpers.Interfaces;
using GameStore.Application.Helpers.Interfaces;
using Gamestore.Application.Helpers.Profiles;
using Gamestore.Application.Services;
using Gamestore.Application.Services.Interfaces;
using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Repositories;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Models.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Gamestore.WebApi.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IGenerateGameFile, GameFileGenerator>();
        services.AddSingleton<IPdfInvoiceGenerator, PdfInvoiceGenerator>();
        services.AddScoped<IKeyGenerator, UniqueKeyGenerator>();

        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IPlatformService, PlatformService>();
        services.AddScoped<IPublisherService, PublisherService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderItemService, OrderItemService>();

        services.AddScoped<IPaymentStrategy, BankPaymentStrategy>();
        services.AddScoped<IPaymentStrategy, IBoxPaymentStrategy>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPaymentStrategy, VisaPaymentStrategy>();

        services.AddAutoMapper(typeof(GameProfile));

        return services;
    }

    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IPlatformRepository, PlatformRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        return services;
    }

    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GamestoreDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GamestoreDb")));
        return services;
    }

    public static IServiceCollection ConfigurePayments(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("PaymentSettings");
        services.Configure<PaymentSettings>(config);
        return services;
    }

    public static IServiceCollection ConfigurePaymentOptions(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetSection("PaymentOptions");
        services.Configure<PaymentOptions>(config);
        return services;
    }
}
