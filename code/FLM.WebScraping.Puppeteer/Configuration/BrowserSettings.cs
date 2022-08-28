using System;
using System.Runtime.InteropServices;

namespace FLM.WebScraping.Puppeteer.Configuration
{
    /// <summary>
    /// Settings for the instance of the browser which will be used.
    /// </summary>
    public class BrowserSettings
    {
        /// <summary>
        /// Decides whether the browser is headless or not.
        /// <br>Defaults to true.</br>
        /// </summary>
        public bool IsBrowserHeaderless { get; set; } = true;

        /// <summary>
        /// Decides whether the browser which is created should be in incognito mode or not.
        /// <br>Defaults to false.</br>
        /// </summary>
        public bool IsIncognito { get; set; } = false;

        /// <summary>
        /// The port on which the created browser should listen for remote debugging sessions.
        /// <br>Defaults to 9222.</br>
        /// </summary>
        public ushort RemoteDebuggingPort { get; set; } = 9222;

        /// <summary>
        /// The path where to save temporary browser session files.
        /// <br>Defaults to "%temp%\PuppeteerTemp-&lt;RANDOM_NUMBER&gt;" on Windows.</br>
        /// <br>Defaults to "/tmp/PuppeteerTemp-&lt;RANDOM_NUMBER&gt;" on Linux.</br>
        /// </summary>
        public string UserDataPath { get; set; }

        /// <summary>
        /// Decides whether --no-sandbox is passed as an argument or not.
        /// <br>Defaults to false on Windows.</br>
        /// <br>Defaults to true on Linux.</br>
        /// </summary>
        public bool IsNoSandbox { get; set; }

        /// <summary>
        /// If set, it will be used to connect to an instance of browser that is already running,
        /// through the remote debugger url.
        /// <br>Falls back to creating a browser if the connection fails.</br>
        /// <br>Template: http://localhost:1234.</br>
        /// </summary>
        public string RemoteDebuggingUrl { get; set; }

        /// <summary>
        /// Defaults whether to use the installed version of the browser or not.
        /// If false, it will use the downloaded version, or download the latest version if none exists.
        /// <br>Defaults to false.</br>
        /// </summary>
        public bool UseInstalledVersion { get; set; } = false;

        /// <summary>
        /// The path to the installed version of the browser, including the executable name.
        /// Set <see cref="UseInstalledVersion"/> to true to use this.
        /// <br>Defaults to "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" on Windows.</br>
        /// <br>Defaults to "/usr/bin/chromium-browser" on Linux.</br>
        /// </summary>
        public string ExecutablePath { get; set; }

        /// <summary>
        /// Decides whether it creates a new browser tab or not.
        /// If false, it will use the last created page, or create a new one if none exist.
        /// <br>Defaults to true.</br>
        /// </summary>
        public bool ShouldCreateTab { get; set; } = true;

        /// <summary>
        /// The page's viewport width.
        /// <br>Defaults to 1920.</br>
        /// </summary>
        public int ViewportWidth { get; set; } = 1920;

        /// <summary>
        /// The page's viewport height.
        /// <br>Defaults to 1080.</br>
        /// </summary>
        public int ViewportHeight { get; set; } = 1080;

        /// <summary>
        /// Intializez a new instance of the <see cref="BrowserSettings"/> class.
        /// </summary>
        public BrowserSettings()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                UserDataPath = $@"%temp%\PuppeteerTemp-{Guid.NewGuid()}";
                IsNoSandbox = false;
                ExecutablePath = @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe";
                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                UserDataPath = $"/tmp/PuppeteerTemp-{Guid.NewGuid()}";
                IsNoSandbox = true;
                ExecutablePath = "/usr/bin/chromium-browser";
                return;
            }

            throw new NotImplementedException("No implementation found for the current OS platform.");
        }
    }
}