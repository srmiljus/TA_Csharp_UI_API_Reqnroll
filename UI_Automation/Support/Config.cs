using System.Text.Json;
using System.Text.Json.Serialization;

namespace UI_Automation.Support
{
    public class Config
    {
        public string BaseUrl { get; set; }
        public string Browser { get; set; }
        public bool Headless { get; set; } = true;
        public bool Incognito { get; set; } = true;
        public bool UseRemote { get; set; } = false;
        public string RemoteUrl { get; set; } = "http://localhost:4444/wd/hub";

        private static Config _instance;
        private static readonly object _lock = new();

        [JsonConstructor]
        public Config(
            string baseUrl,
            string browser,
            bool headless = true,
            bool incognito = true,
            bool useRemote = false,
            string remoteUrl = "http://localhost:4444/wd/hub")
        {
            BaseUrl = baseUrl;
            Browser = browser;
            Headless = headless;
            Incognito = incognito;
            UseRemote = useRemote;
            RemoteUrl = remoteUrl;
        }

        public static Config Load(string fileName = "appsettings.json")
        {
            if (_instance != null)
                return _instance;

            lock (_lock)
            {
                if (_instance != null)
                    return _instance;

                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var filePath = Path.Combine(basePath, fileName);

                if (!File.Exists(filePath))
                    throw new FileNotFoundException($"Configuration file '{fileName}' not found at '{filePath}'.");

                var json = File.ReadAllText(filePath);

                _instance = JsonSerializer.Deserialize<Config>(json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (_instance == null)
                    throw new InvalidOperationException("Failed to deserialize configuration.");

                // Allow environment variable overrides for CI
                var envBrowser = Environment.GetEnvironmentVariable("BROWSER");
                if (!string.IsNullOrWhiteSpace(envBrowser))
                    _instance.Browser = envBrowser;

                var envHeadless = Environment.GetEnvironmentVariable("HEADLESS");
                if (!string.IsNullOrWhiteSpace(envHeadless))
                    _instance.Headless = envHeadless.Equals("true", StringComparison.OrdinalIgnoreCase);

                return _instance;
            }
        }
    }
}
