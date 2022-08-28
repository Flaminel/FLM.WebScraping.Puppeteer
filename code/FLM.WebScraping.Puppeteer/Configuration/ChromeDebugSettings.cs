using System.Text.Json.Serialization;

namespace FLM.WebScraping.Puppeteer.Configuration
{
    internal class ChromeDebugSettings
    {
        /*
        "Browser": "Chrome/88.0.4324.146",
        "Protocol-Version": "1.3",
        "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.146 Safari/537.36",
        "V8-Version": "8.8.278.14",
        "WebKit-Version": "537.36 (@406dc88511162d6598242f2c709be1414a042fb0)",
        "webSocketDebuggerUrl": "ws://127.0.0.1:9222/devtools/browser/290343f6-17f4-4480-bfb7-8f1b7d9d83c1"
        */

        [JsonPropertyName("webSocketDebuggerUrl")]
        public string WebSocketDebuggerUrl { get; set; }
    }
}