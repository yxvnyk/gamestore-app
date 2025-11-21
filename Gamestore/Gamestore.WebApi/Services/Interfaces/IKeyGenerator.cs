namespace Gamestore.WebApi.Services.Interfaces;

public interface IKeyGenerator
{
    Task<string> GenerateUniqueKeyAsync(string name);
}
