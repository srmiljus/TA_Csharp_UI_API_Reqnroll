using Allure.Net.Commons;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using Reqnroll.BoDi;
using System.Runtime.InteropServices;
using System.Text;
using UI_Automation.Enums;
using UI_Automation.Support;

//[assembly: Parallelizable(ParallelScope.Self | ParallelScope.Children)]
//[assembly: Parallelizable(ParallelScope.Fixtures)]
//[assembly: Parallelizable(ParallelScope.All)]
//[assembly: LevelOfParallelism(2)]

namespace UI_Automation.Setup
{
    [Binding]
    public class TestSetup : IDisposable
    {
        private readonly IObjectContainer _container;
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        private static Config _config;

        public TestSetup(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _container = container;
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void GlobalSetup()
        {
            _config = Config.Load();
            AllureLifecycle.Instance.CleanupResultDirectory();

            var resultsDir = AllureLifecycle.Instance.ResultsDirectory
                             ?? Path.Combine(AppContext.BaseDirectory, "allure-results");
            Directory.CreateDirectory(resultsDir);


            var osName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "Windows" :
                         RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "macOS" : "Linux";

            File.WriteAllLines(Path.Combine(resultsDir, "environment.properties"), new[]
            {
                "env=QA",
                $"baseUrl={_config.BaseUrl}",
                $"browser={_config.Browser}",
                $"os={osName}",
                "framework=Reqnroll + NUnit + Selenium"
            });

            File.WriteAllText(Path.Combine(resultsDir, "executor.json"),
                """
                {
                  "name": "Local Run",
                  "type": "other",
                  "buildName": "UI_Automation"
                }
                """, Encoding.UTF8);

            File.WriteAllText(Path.Combine(resultsDir, "categories.json"),
                """
                [
                  {
                    "name": "Product defects",
                    "matchedStatuses": ["failed"]
                  },
                  {
                    "name": "Test defects",
                    "matchedStatuses": ["broken"]
                  },
                  {
                    "name": "Ignored tests",
                    "matchedStatuses": ["skipped"]
                  }
                ]
                """, Encoding.UTF8);
        }

        [BeforeScenario]
        public void InitializeWebDriver()
        {
            // Remote toggle (Docker/Grid)
            var useRemote = IsRemoteEnabled();

            _driver = CreateWebDriver(
                _config.Browser,
                _config.Headless,
                _config.Incognito,
                useRemote
            );

            try { _driver.Manage().Window.Maximize(); } catch { }


            _container.RegisterInstanceAs<IWebDriver>(_driver);
            _scenarioContext["driver"] = _driver;

            Logger.Log($"Initialized WebDriver for browser: {_config.Browser}, Headless: {_config.Headless}, Incognito: {_config.Incognito}, Remote: {useRemote}");
        }

        private static bool IsRemoteEnabled()
        {
            var raw = Environment.GetEnvironmentVariable("USE_REMOTE") ?? "false";
            return raw.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                   raw.Equals("1", StringComparison.OrdinalIgnoreCase) ||
                   raw.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        private static string GetRemoteUrl()
        {
            // docker-compose: http://selenium:4444/wd/hub
            // local host:     http://localhost:4444/wd/hub
            return Environment.GetEnvironmentVariable("SELENIUM_REMOTE_URL")
           ?? "http://selenium:4444/wd/hub";

        }

        private static IWebDriver CreateWebDriver(string browserName, bool headless, bool incognito, bool useRemote)
        {
            //If remote is enabled → use Selenium Grid (docker chrome)
            if (useRemote)
            {
                var remoteUrl = GetRemoteUrl();

                // Remote side is typically Chrome in selenium/standalone-chrome
                // Keep options aligned with your existing flags.
                var options = new ChromeOptions();
                if (headless) options.AddArgument("--headless=new");
                if (incognito) options.AddArgument("--incognito");

                // Optional stability options (safe for Docker)
                options.AddArgument("--no-sandbox");
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--window-size=1920,1080");

                return new RemoteWebDriver(new Uri(remoteUrl), options);
            }

            if (Enum.TryParse(browserName, true, out BrowserType browser))
            {
                switch (browser)
                {
                    case BrowserType.Chrome:
                        var chromeOptions = new ChromeOptions();
                        if (headless) chromeOptions.AddArgument("--headless=new");
                        if (incognito) chromeOptions.AddArgument("--incognito");
                        chromeOptions.AddArgument("--no-sandbox");
                        chromeOptions.AddArgument("--disable-dev-shm-usage");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                        return new ChromeDriver(chromeOptions);

                    case BrowserType.Firefox:
                        var firefoxOptions = new FirefoxOptions();
                        if (headless) firefoxOptions.AddArgument("--headless");
                        if (incognito) firefoxOptions.AddArgument("-private");
                        return new FirefoxDriver(firefoxOptions);

                    case BrowserType.Edge:
                        var edgeOptions = new EdgeOptions();
                        if (headless) edgeOptions.AddArgument("headless");
                        if (incognito) edgeOptions.AddArgument("inprivate");
                        return new EdgeDriver(edgeOptions);

                    case BrowserType.Safari:
                        if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                            throw new PlatformNotSupportedException("Safari is only supported on macOS.");
                        if (headless)
                            throw new NotSupportedException("Safari does not support headless mode via Selenium.");
                        if (incognito)
                            throw new NotSupportedException("Safari does not support incognito mode via Selenium.");
                        return new OpenQA.Selenium.Safari.SafariDriver();
                }
            }

            throw new ArgumentOutOfRangeException(nameof(browserName), $"Browser '{browserName}' is not supported.");
        }

        public void NavigateToBaseUrl()
        {
            string baseUrl = _config.BaseUrl ?? "https://store.steampowered.com/";
            _driver.Navigate().GoToUrl(baseUrl);
        }

        [AfterScenario(Order = -10000)]
        public void Dispose()
        {
            if (_driver == null) return;

            try { _driver.Quit(); }
            catch { }
            finally
            {
                try { _driver.Dispose(); } catch { }
                _driver = null;
            }
        }
    }
}
