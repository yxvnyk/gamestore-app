// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Gamestore.DataAccess.Context;
using Gamestore.DataAccess.Repositories;
using Gamestore.DataAccess.Repositories.Interfaces;
using Gamestore.WebApi.Helpers;
using Gamestore.WebApi.Services;
using Gamestore.WebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(GameProfile));

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IGameDatabaseService, GameDatabaseService>();
builder.Services.AddScoped<IGenreDatabaseService, GenreDatabaseService>();
builder.Services.AddScoped<IPlatformDatabaseService, PlatformDatabaseService>();

builder.Services.AddControllers();
builder.Services.AddDbContext<GamestoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GamestoreDb")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
