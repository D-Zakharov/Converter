using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace Converter.ConverterService.Services.Converters;

internal sealed class HtmlToPdfConverter : IConverter, IDisposable
{
    private IBrowser? Browser { get; set; }

    public async Task<string> Convert(string inputFilePath)
    {
        if (Browser is null)
        {
            throw new Exception($"Browser is not initialized in {nameof(HtmlToPdfConverter)}");
            //await Init();
        }

        var outputFilePath = Path.GetTempFileName();

        await using var page = await Browser!.NewPageAsync();
        await page.GoToAsync(inputFilePath);
        await page.PdfAsync(outputFilePath);
        await page.CloseAsync();

        return outputFilePath;
    }

    public async Task Init()
    {
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
        Browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
    }

    public void Dispose()
    {
        if (Browser is not null)
            Browser.Dispose();
    }
}
