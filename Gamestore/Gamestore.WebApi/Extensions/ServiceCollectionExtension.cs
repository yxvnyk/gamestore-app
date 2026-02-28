using Gamestore.Application.Helpers.Generators;
using Gamestore.Application.Helpers.Interfaces;
using GameStore.Application.Helpers.Interfaces;
using Gamestore.Application.Helpers.Profiles;
using Gamestore.Application.Services;
using Gamestore.Application.Services.Interfaces;
using Gamestore.Application.Services.Interfaces.Payments;
using Gamestore.Application.Services.Payments;
using Gamestore.Application.Services.Payments.Strategies;
using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Northwind.Context;
using Gamestore.DataAccess.Northwind.Repositories;
using Gamestore.DataAccess.Northwind.Repositories.Interfaces;
using Gamestore.DataAccess.Repositories;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.Domain.Models.Configuration;
using Gamestore.Infrastructure.ExternalServices;
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
        services.AddScoped<ICommentService, CommentService>();

        services.AddScoped<IPaymentStrategy, BankPaymentStrategy>();
        services.AddScoped<IPaymentStrategy, BoxPaymentStrategy>();
        services.AddScoped<IPaymentStrategy, VisaPaymentStrategy>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IPaymentService, PaymentService>();

        services.AddScoped<IShipperService, ShipperService>();

        services.AddAutoMapper(typeof(GameProfile));

        return services;
    }

    public static IServiceCollection AddNorthwindDataAccess(this IServiceCollection services)
    {
        services.AddScoped<INorthwindOrderRepository, NorthwindOrderRepository>();
        services.AddScoped<INorthwindShipperRepository, NorthwindShipperRepository>();

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
        services.AddScoped<ICommentRepository, CommentRepository>();

        return services;
    }

    public static IServiceCollection ConfigureGamestoreDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GamestoreDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GamestoreDb")));

        return services;
    }

    public static IServiceCollection ConfigureNorthwindDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<NorthwindDatabaseSettings>(configuration.GetSection("NorthwindDb"));
        services.AddSingleton<NorthwindDbContext>();

        return services;
    }

    public static IServiceCollection ConfigurePaymentService(this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["ApiSettings:PaymentService"];
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new InvalidOperationException("Payment Service URL is missing in configuration!");
        }

        services.AddHttpClient<IPaymentProxy, PaymentProxy>(client =>
        {
            client.BaseAddress = new Uri(url);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

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
