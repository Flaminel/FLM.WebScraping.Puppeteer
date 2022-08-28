using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FLM.WebScraping.Puppeteer.Configuration;
using FLM.WebScraping.Puppeteer.Providers;
using PuppeteerSharp;
using System;
using System.Threading.Tasks;

namespace FLM.WebScraping.Puppeteer.Browsing.Interfaces;

/// <summary>
/// Provides methods the work with a browser page (a.k.a. browser tab).
/// Support for OSX is not provided. Some things might work, some might not.
/// </summary>
public interface IBrowserTab : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// The page used for navigation.
    /// </summary>
    IPage Page { get; }

    /// <summary>
    /// Initializez the browser and the page. This should be called before doing anything else.
    /// Keep in mind that, if Chromium has not been downloaded using <see cref="BrowserProvider.DownloadBrowserAsync(string)"/> yet,
    /// this method will take more time to complete as it will download the default revision of the browser.
    /// </summary>
    /// <param name="browserSettings">The settings for creating or using a browser.</param>
    Task SetupAsync(BrowserSettings browserSettings = null);

    /// <summary>
    /// Navigates to an url.
    /// </summary>
    /// <param name="url">The url to navigate to.</param>
    Task GoToAsync(string url);

    /// <summary>
    /// Returns a list of the elements within the document (using depth-first pre-order
    /// traversal of the document's nodes) that match the specified group of selectors.
    /// </summary>
    /// <param name="selector">The selector to use.</param>
    /// <returns>A non-live NodeList of element objects.</returns>
    Task<IHtmlCollection<IElement>> GetElementsAsync(string selector);

    /// <summary>
    /// Writes the provided keys to the first element found with the provided selector.
    /// </summary>
    /// <param name="selector">The selector to use.</param>
    /// <param name="keys">The keys to write.</param>
    Task SendKeysAsync(string selector, string keys);

    /// <summary>
    /// Gets the current document by parsing the page content.
    /// </summary>
    /// <returns>A new <see cref="IHtmlDocument"/> from the page content.</returns>
    Task<IHtmlDocument> GetCurrentDocumentAsync();
}