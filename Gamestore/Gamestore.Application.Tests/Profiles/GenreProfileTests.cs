using AutoMapper;
using Gamestore.Application.Helpers.Profiles;
using Gamestore.DataAccess.Entities;
using Gamestore.Domain.Models.DTO;

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
        var dto = new GenreCreateDto
        {
            Name = "Shooter",
            ParentGenreId = Guid.NewGuid(),
        };

        var entity = _mapper.Map<Genre>(dto);

        Assert.Equal("Shooter", entity.Name);
        Assert.Equal(dto.ParentGenreId, entity.ParentGenreId);
        Assert.Equal(Guid.Empty, entity.Id);
    }

    [Fact]
    public void Map_Genre_To_GenreDto_Success()
    {
        var entity = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Action",
        };

        var dto = _mapper.Map<GenreDto>(entity);

        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal("Action", dto.Name);
    }

    [Fact]
    public void Map_Genre_To_GenreFullDto_IncludesParent()
    {
        var entity = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "RPG",
            ParentGenreId = Guid.NewGuid(),
        };

        var dto = _mapper.Map<GenreFullDto>(entity);

        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal("RPG", dto.Name);
        Assert.Equal(entity.ParentGenreId, dto.ParentGenreId);
    }

    [Fact]
    public void Map_GenreUpdateDto_To_Genre_UpdatesName_AndParent()
    {
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

        _mapper.Map(dto, entity);

        Assert.Equal("New", entity.Name);
        Assert.Equal(dto.ParentGenreId, entity.ParentGenreId);
    }

    [Fact]
    public void Map_GenreUpdateDto_DoesNotOverride_Id()
    {
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

        _mapper.Map(dto, entity);

        Assert.Equal(originalId, entity.Id);
    }

    [Fact]
    public void Map_GenreUpdateDto_AllowsPartialUpdate_WhenNameIsNull()
    {
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

        _mapper.Map(dto, entity);

        Assert.Equal("Before", entity.Name);
        Assert.Equal(dto.ParentGenreId, entity.ParentGenreId);
    }
}
