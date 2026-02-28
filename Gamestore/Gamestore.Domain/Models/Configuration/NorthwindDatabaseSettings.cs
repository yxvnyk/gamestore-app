namespace Gamestore.Domain.Models.Configuration;

public class NorthwindDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
}
