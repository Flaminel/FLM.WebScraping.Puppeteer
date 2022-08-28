using System.Collections.Generic;
using System.Text;
using FLM.WebScraping.Puppeteer.Configuration;

namespace FLM.WebScraping.Puppeteer.Extensions
{
    /// <summary>
    /// Extension methods class for <see cref="BrowserSettings"/>.
    /// </summary>
    public static class BrowserSettingsExtensions
    {
        /// <summary>
        /// Converts the properties that can be converted to command arguments.
        /// </summary>
        /// <param name="browserSettings">The browser settings to convert.</param>
        /// <returns>A string containing the arguments.</returns>
        public static string ToCommandArguments(this BrowserSettings browserSettings)
        {
            StringBuilder arguments = new();

            if (browserSettings.IsBrowserHeaderless)
            {
                _ = arguments.Append("--headless ");
            }

            if (browserSettings.IsIncognito)
            {
                _ = arguments.Append("--incognito ");
            }

            if (browserSettings.IsNoSandbox)
            {
                // TODO check if this is till needed
                _ = arguments.Append(" --no-sandbox");
            }

            _ = arguments.Append("--new-window ");
            _ = arguments.Append($"--remote-debugging-port={browserSettings.RemoteDebuggingPort} ");
            _ = arguments.Append($"--user-data-dir={browserSettings.UserDataPath}");

            return arguments.ToString();
        }

        /// <summary>
        /// Converts the properties that can be converted to command arguments.
        /// </summary>
        /// <param name="browserSettings">The browser settings to convert.</param>
        /// <returns>A list containing the arguments.</returns>
        public static IList<string> ToCommandArgumentsList(this BrowserSettings browserSettings)
        {
            List<string> arguments = new();

            if (browserSettings.IsBrowserHeaderless)
            {
                arguments.Add("--headless");
            }

            if (browserSettings.IsIncognito)
            {
                arguments.Add("--incognito");
            }

            if (browserSettings.IsNoSandbox)
            {
                // TODO check if this is till needed
                arguments.Add("--no-sandbox");
            }

            arguments.Add("--new-window");
            arguments.Add($"--remote-debugging-port={browserSettings.RemoteDebuggingPort}");
            arguments.Add($"--user-data-dir={browserSettings.UserDataPath}");

            return arguments;
        }
    }
}