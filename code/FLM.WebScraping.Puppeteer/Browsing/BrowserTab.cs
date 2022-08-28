using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using FLM.WebScraping.Puppeteer.Browsing.Interfaces;
using FLM.WebScraping.Puppeteer.Configuration;
using FLM.WebScraping.Puppeteer.Providers.Interfaces;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FLM.WebScraping.Puppeteer.Browsing;

/// <inheritdoc cref="IBrowserTab"/>
public class BrowserTab : IBrowserTab
{
    private readonly IBrowserProvider _browserProvider;
    private readonly IHtmlParser _htmlParser;
    private readonly bool _isPartOfSomethingBigger;
    private IBrowser _browser;

    /// <inheritdoc/>
    public IPage Page { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserTab"/> class.
    /// </summary>
    /// <param name="browserProvider">The provider of browsers.</param>
    /// <param name="browser">The browser to use.</param>
    public BrowserTab(IBrowserProvider browserProvider, Browser browser = null)
    {
        _browserProvider = browserProvider ?? throw new ArgumentNullException(nameof(browserProvider));

        if (browser != null)
        {
            _browser = browser;
            _isPartOfSomethingBigger = true;
        }

        _htmlParser = BrowsingContext.New(AngleSharp.Configuration.Default)
            .GetService<IHtmlParser>();
    }

    /// <inheritdoc/>
    public async Task SetupAsync(BrowserSettings browserSettings = null)
    {
        browserSettings ??= new();

        _browser ??= await _browserProvider.CreateBrowserAsync(browserSettings);

        IPage[] availablePages = await _browser.PagesAsync();

        Page = browserSettings.ShouldCreateTab || !availablePages.Any() ?
            await _browser.NewPageAsync()
            : availablePages[^1];

        await Page.SetViewportAsync(new ViewPortOptions()
        {
            Width = browserSettings.ViewportWidth,
            Height = browserSettings.ViewportHeight
        });
    }

    /// <inheritdoc/>
    public async Task GoToAsync(string url) => await Page.GoToAsync(url, new NavigationOptions()
    {
        WaitUntil = new[] { WaitUntilNavigation.DOMContentLoaded }
    });

    /// <inheritdoc/>
    public async Task<IHtmlCollection<IElement>> GetElementsAsync(string selector)
    {
        IHtmlDocument document = await GetCurrentDocumentAsync();

        return document.QuerySelectorAll(selector);
    }

    /// <inheritdoc/>
    public async Task SendKeysAsync(string selector, string keys) =>
        // typing in a focused item has higher chance of success
        // also, this will work on Angular/React pages - the other one won't
        await Page.TypeAsync(selector, keys, new TypeOptions() { Delay = 12 });

    /// <inheritdoc/>
    public async Task<IHtmlDocument> GetCurrentDocumentAsync()
    {
        string content;

        try
        {
            content = await Page.GetContentAsync();
        }
        catch (EvaluationFailedException)
        {
            // Retry after random fail to make sure there's a problem.
            await GoToAsync(Page.Url);

            content = await Page.GetContentAsync();
        }

        return await _htmlParser.ParseDocumentAsync(content);
    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        Page?.Dispose();

        if (!_isPartOfSomethingBigger)
        {
            _browser?.Dispose();
        }

        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public virtual async ValueTask DisposeAsync()
    {
        if (Page != null)
        {
            await Page.DisposeAsync();
        }

        if (!_isPartOfSomethingBigger)
        {
            if (_browser != null)
            {
                await _browser.DisposeAsync();
            }
        }

        GC.SuppressFinalize(this);
    }
}