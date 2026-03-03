// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Gamestore.WebApi.Extensions;
using Gamestore.WebApi.Helpers;
using Gamestore.WebApi.Helpers.Middleware;
using QuestPDF.Infrastructure;
using Serilog;

QuestPDF.Settings.License = LicenseType.Community;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders(CustomHeaders.TotalGamesCount);
    });
});

builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));

builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<RequestDetailsLoggingMiddleware>();

builder.Services.AddTransient<TotalGamesHeaderMiddleware>();

builder.Services.AddDomainServices();
builder.Services.ConfigureGamestoreDatabase(builder.Configuration);
builder.Services.ConfigureNorthwindDatabase(builder.Configuration);
builder.Services.AddDataAccess();
builder.Services.AddNorthwindDataAccess();
builder.Services.AddApplicationServices();
builder.Services.ConfigurePayments(builder.Configuration);
builder.Services.ConfigurePaymentOptions(builder.Configuration);
builder.Services.ConfigurePaymentService(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

await app.InitializeNorthwindDatabaseAsync();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestDetailsLoggingMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("AllowAll");
app.UseMiddleware<TotalGamesHeaderMiddleware>();

app.UseAuthorization();

app.UseResponseCaching();

app.MapControllers();

app.Run();
