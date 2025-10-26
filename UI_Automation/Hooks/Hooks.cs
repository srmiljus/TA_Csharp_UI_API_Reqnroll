using Allure.Net.Commons;
using OpenQA.Selenium;


namespace UI_Automation.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly IWebDriver _driver;

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
      
            _driver = scenarioContext.ContainsKey("driver") ? (IWebDriver)scenarioContext["driver"] : null;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            Console.WriteLine("=== Starting UI test run with Allure.Reqnroll ===");
        }

        [AfterStep]
        public void AfterStep()
        {
            if (_scenarioContext.TestError != null && _driver != null)
            {
                TakeScreenshot("AfterStepFailure");
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (_scenarioContext.TestError != null && _driver != null)
            {
                TakeScreenshot("AfterScenarioFailure");
            }

            _driver?.Quit();
        }

        private void TakeScreenshot(string namePrefix)
        {
            try
            {
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                var fileName = $"{namePrefix}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
         
                screenshot.SaveAsFile(path);

                AllureApi.AddAttachment(fileName, "image/png", path);
                Console.WriteLine($"Screenshot saved and attached: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to capture screenshot: {ex.Message}");
            }
        }
    }
}
