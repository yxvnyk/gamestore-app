namespace GameStore.Application.Helpers.Interfaces;

public interface IKeyGenerator
{
    Task<string> GenerateUniqueKeyAsync(string name);
}
