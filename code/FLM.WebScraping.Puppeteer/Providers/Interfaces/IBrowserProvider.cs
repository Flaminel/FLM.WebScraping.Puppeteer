using FLM.WebScraping.Puppeteer.Configuration;
using PuppeteerSharp;
using System.Threading.Tasks;

namespace FLM.WebScraping.Puppeteer.Providers.Interfaces;

/// <summary>
/// Provides a means to download and create browser instances.
/// </summary>
public interface IBrowserProvider
{
    /// <summary>
    /// Downloads the browser required to do Puppeteer work.
    /// </summary>
    /// <param name="revision">The revision to download. Defaults to <see cref="BrowserFetcher.DefaultChromiumRevision"/>.</param>
    Task<RevisionInfo> DownloadBrowserAsync(string revision = BrowserFetcher.DefaultChromiumRevision);

    /// <summary>
    /// Creates a new browser.
    /// </summary>
    /// <param name="browserSettings">Settings for the instance of the browser which will be used.</param>
    /// <returns>A new <see cref="Browser"/> instance.</returns>
    Task<IBrowser> CreateBrowserAsync(BrowserSettings browserSettings);
}