namespace Gamestore.Domain.Models.DTO.File;

public class FileDto
{
    public string FileName { get; set; } = null!;

    public byte[] Content { get; set; } = null!;
}
