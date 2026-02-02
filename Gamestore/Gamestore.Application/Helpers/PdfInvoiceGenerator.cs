using Gamestore.Application.Helpers.Interfaces;
using Gamestore.Domain.Models.DTO.Payment;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Gamestore.Application.Helpers;

public class PdfInvoiceGenerator : IPdfInvoiceGenerator
{
    private const string InvoiceTitle = "BANK TRANSFER INVOICE";
    private const string GeneratedOnLabel = "Generated on: ";
    private const string OrderIdLabel = "Order ID:";
    private const string CustomerIdLabel = "Customer ID:";
    private const string DateLabel = "Issue Date:";
    private const string ValidUntilLabel = "Valid Until:";
    private const string TotalLabel = "Total Amount to Pay:";
    private const string PageSeparator = " / ";
    private const string DateFormat = "d";

    private static readonly TextStyle TitleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);
    private static readonly TextStyle NormalStyle = TextStyle.Default.FontSize(12);
    private static readonly TextStyle LabelStyle = TextStyle.Default.FontSize(12).FontColor(Colors.Grey.Darken2);
    private static readonly TextStyle WarningStyle = TextStyle.Default.FontSize(12).FontColor(Colors.Red.Medium);
    private static readonly TextStyle TotalLabelStyle = TextStyle.Default.FontSize(14);
    private static readonly TextStyle TotalValueStyle = TextStyle.Default.FontSize(14).Bold();
    private static readonly TextStyle FooterStyle = TextStyle.Default.FontSize(10).FontColor(Colors.Grey.Medium);

    public byte[] GenerateInvoice(InvoiceDataDto data)
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                ConfigurePage(page);

                page.Header().Element(ComposeHeader);
                page.Content().Element(c => ComposeContent(c, data));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ConfigurePage(PageDescriptor page)
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(NormalStyle);
    }

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text(InvoiceTitle).Style(TitleStyle);

                column.Item().Text(text =>
                {
                    text.Span(GeneratedOnLabel).FontColor(Colors.Grey.Medium);
                    text.Span(DateTime.UtcNow.ToString(DateFormat));
                });
            });
        });
    }

    private static void ComposeContent(IContainer container, InvoiceDataDto data)
    {
        container.PaddingVertical(1, Unit.Centimetre).Column(column =>
        {
            column.Spacing(10);

            column.Item().Element(c => ComposeInfoTable(c, data));

            column.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

            column.Item().Element(c => ComposeTotal(c, data.Amount));
        });
    }

    private static void ComposeInfoTable(IContainer container, InvoiceDataDto data)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(120);
                columns.RelativeColumn();
            });

            void AddRow(string label, string value, TextStyle? style = null)
            {
                table.Cell().Text(label).Style(LabelStyle);
                table.Cell().Text(value).Style(style ?? NormalStyle);
            }

            AddRow(OrderIdLabel, data.OrderId.ToString());
            AddRow(CustomerIdLabel, data.CustomerId.ToString());
            AddRow(DateLabel, data.CreatedAt.ToString(DateFormat));
            AddRow(ValidUntilLabel, data.ValidUntil.ToString(DateFormat), WarningStyle);
        });
    }

    private static void ComposeTotal(IContainer container, double amount)
    {
        container.Row(row =>
        {
            row.RelativeItem().Text(TotalLabel).Style(TotalLabelStyle);
            row.RelativeItem().AlignRight().Text($"{amount:C}").Style(TotalValueStyle);
        });
    }

    private static void ComposeFooter(IContainer container) =>
    container.AlignCenter().Text(static text =>
    {
        text.DefaultTextStyle(FooterStyle);

        text.CurrentPageNumber();
        text.Span(PageSeparator);
        text.TotalPages();
    });
}