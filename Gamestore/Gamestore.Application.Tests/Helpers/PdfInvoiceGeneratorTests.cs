using Gamestore.Application.Helpers.Generators;
using Gamestore.Domain.Models.DTO.Payment;
using QuestPDF.Infrastructure;

namespace Gamestore.Application.Tests.Helpers;

public class PdfInvoiceGeneratorTests
{
    [Fact]
    public void GenerateInvoice_ValidData_ReturnsPdfBytes()
    {
        QuestPDF.Settings.License = LicenseType.Community;

        // Arrange
        var generator = new PdfInvoiceGenerator();
        var data = new BankInvoiceDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100.50,
            DateTime.Now,
            DateTime.Now.AddDays(30));

        // Act
        var result = generator.GenerateInvoice(data);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Length > 0);
        Assert.Equal((byte)0x25, result[0]);
    }
}