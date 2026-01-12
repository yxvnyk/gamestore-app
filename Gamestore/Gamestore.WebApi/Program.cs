// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Gamestore.WebApi.Extensions;
using Gamestore.WebApi.Helpers;
using Gamestore.WebApi.Helpers.Middleware;

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

builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<TotalGamesHeaderMiddleware>();

builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.AddDataAccess();
builder.Services.AddApplicationServices();

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
