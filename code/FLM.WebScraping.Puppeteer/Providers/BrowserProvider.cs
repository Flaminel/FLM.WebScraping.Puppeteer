using FLM.WebScraping.Puppeteer.Configuration;
using FLM.WebScraping.Puppeteer.Extensions;
using FLM.WebScraping.Puppeteer.Providers.Interfaces;
using PuppeteerSharp;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FLM.WebScraping.Puppeteer.Providers;

/// <inheritdoc cref="IBrowserProvider"/>
public class BrowserProvider : IBrowserProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly BrowserFetcher _browserFetcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrowserProvider"/> class.
    /// </summary>
    /// <param name="httpClientFactory">The factory that creates instances of <see cref="HttpClient"/>.</param>
    /// <param name="browserFetcher">The browser fetcher to use.</param>
    public BrowserProvider(IHttpClientFactory httpClientFactory, BrowserFetcher browserFetcher = null)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _browserFetcher = browserFetcher ?? new();
    }

    /// <inheritdoc/>
    public async Task<RevisionInfo> DownloadBrowserAsync(string revision = BrowserFetcher.DefaultChromiumRevision)
        => await _browserFetcher.DownloadAsync(revision);

    /// <inheritdoc/>
    public async Task<IBrowser> CreateBrowserAsync(BrowserSettings browserSettings)
    {
        if (!string.IsNullOrEmpty(browserSettings.RemoteDebuggingUrl))
        {
            try
            {
                ChromeDebugSettings chromeDebugSettings = await GetChromeDebugSettingsAsync(browserSettings.RemoteDebuggingUrl);

                if (chromeDebugSettings is not null)
                {
                    return await PuppeteerSharp.Puppeteer.ConnectAsync(new ConnectOptions()
                    {
                        BrowserWSEndpoint = chromeDebugSettings.WebSocketDebuggerUrl
                    });
                }
            }
            catch (ProcessException)
            {
                // Failed to connect to the remote session.
            }
        }

        string executablePath;

        if (browserSettings.UseInstalledVersion && !string.IsNullOrEmpty(browserSettings.ExecutablePath))
        {
            executablePath = browserSettings.ExecutablePath;
        }
        else
        {
            RevisionInfo revisionInfo = !_browserFetcher.LocalRevisions().Any()
                ? await DownloadBrowserAsync()
                : _browserFetcher.RevisionInfo(BrowserFetcher.DefaultChromiumRevision);

            executablePath = revisionInfo.ExecutablePath;
        }

        return await PuppeteerSharp.Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = false, // Will be overwritten by the command argument.
            ExecutablePath = executablePath,
            Args = browserSettings.ToCommandArgumentsList()
                .ToArray()
        });
    }

    private async Task<ChromeDebugSettings> GetChromeDebugSettingsAsync(string remoteDebuggingUrl)
    {
        try
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                return await httpClient
                    .GetFromJsonAsync<ChromeDebugSettings>($"{remoteDebuggingUrl.TrimEnd('/')}/json/version");
            }
        }
        catch (Exception)
        {
            // Invalid JSON
        }

        return null;
    }
}