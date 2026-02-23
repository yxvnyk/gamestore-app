using AutoMapper;
using Gamestore.Application.Helpers.Profiles;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO.Genre;

namespace Gamestore.Application.Tests.Profiles;

public class GenreProfileTests
{
    private readonly IMapper _mapper;

    public GenreProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<GenreProfile>();
        });

        config.AssertConfigurationIsValid();
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Map_GenreCreateDto_To_Genre_Success()
    {
        // Arrange
        var dto = new GenreCreateDto
        {
            Name = "Shooter",
            ParentGenreId = Guid.NewGuid(),
        };

        // Act
        var entity = _mapper.Map<Genre>(dto);

        // Assert
        Assert.Equal("Shooter", entity.Name);
        Assert.Equal(dto.ParentGenreId, entity.ParentGenreId);
        Assert.Equal(Guid.Empty, entity.Id);
    }

    [Fact]
    public void Map_Genre_To_GenreDto_Success()
    {
        // Arrange
        var entity = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Action",
        };

        // Act
        var dto = _mapper.Map<GenreDto>(entity);

        // Assert
        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal("Action", dto.Name);
    }

    [Fact]
    public void Map_Genre_To_GenreFullDto_IncludesParent()
    {
        // Arrange
        var entity = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "RPG",
            ParentGenreId = Guid.NewGuid(),
        };

        // Act
        var dto = _mapper.Map<GenreFullDto>(entity);

        // Assert
        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal("RPG", dto.Name);
        Assert.Equal(entity.ParentGenreId, dto.ParentGenreId);
    }

    [Fact]
    public void Map_GenreUpdateDto_To_Genre_UpdatesName_AndParent()
    {
        // Arrange
        var entity = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Old",
            ParentGenreId = Guid.NewGuid(),
        };

        var dto = new GenreUpdateDto
        {
            Id = entity.Id,
            Name = "New",
            ParentGenreId = Guid.NewGuid(),
        };

        // Act
        _mapper.Map(dto, entity);

        // Assert
        Assert.Equal("New", entity.Name);
        Assert.Equal(dto.ParentGenreId, entity.ParentGenreId);
    }

    [Fact]
    public void Map_GenreUpdateDto_DoesNotOverride_Id()
    {
        // Arrange
        var originalId = Guid.NewGuid();

        var entity = new Genre
        {
            Id = originalId,
            Name = "Old Name",
        };

        var dto = new GenreUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = "Updated Name",
        };

        // Act
        _mapper.Map(dto, entity);

        // Assert
        Assert.Equal(originalId, entity.Id);
    }

    [Fact]
    public void Map_GenreUpdateDto_AllowsPartialUpdate_WhenNameIsNull()
    {
        // Arrange
        var entity = new Genre
        {
            Id = Guid.NewGuid(),
            Name = null,
            ParentGenreId = Guid.NewGuid(),
        };

        var dto = new GenreUpdateDto
        {
            Id = entity.Id,
            Name = "Before",
            ParentGenreId = Guid.NewGuid(),
        };

        // Act
        _mapper.Map(dto, entity);

        // Assert
        Assert.Equal("Before", entity.Name);
        Assert.Equal(dto.ParentGenreId, entity.ParentGenreId);
    }
}