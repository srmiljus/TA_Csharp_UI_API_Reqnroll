using Allure.Net.Commons;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using Reqnroll;
using Reqnroll.BoDi;
using System.Collections.Generic;
using System.IO;
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

            // environment.properties
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
        }

        [BeforeScenario]
        public void InitializeWebDriver()
        {
            _driver = CreateWebDriver(_config.Browser, _config.Headless, _config.Incognito);
            try { _driver.Manage().Window.Maximize(); } catch { }
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            
            _container.RegisterInstanceAs<IWebDriver>(_driver);

            
            _scenarioContext["driver"] = _driver;

            Logger.Log($"Initialized WebDriver for browser: {_config.Browser}, Headless: {_config.Headless}, Incognito: {_config.Incognito}");
        }

        private static IWebDriver CreateWebDriver(string browserName, bool headless, bool incognito)
        {
            if (Enum.TryParse(browserName, true, out BrowserType browser))
            {
                switch (browser)
                {
                    case BrowserType.Chrome:
                        var chromeOptions = new ChromeOptions();
                        if (headless) chromeOptions.AddArgument("--headless=new");
                        if (incognito) chromeOptions.AddArgument("--incognito");
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
            throw new ArgumentOutOfRangeException(nameof(browser), $"Browser '{browser}' is not supported.");
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

