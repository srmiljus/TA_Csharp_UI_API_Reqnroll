using System.Text;
using Allure.Net.Commons;
using OpenQA.Selenium;
using Reqnroll;

namespace UI_Automation.Hooks
{
    [Binding]
    public class AllureStepScreenshotHook
    {
        private readonly ScenarioContext _scenarioContext;

        public AllureStepScreenshotHook(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [AfterStep(Order = 9999)]
        public void AfterStep()
        {
   
            if (_scenarioContext.TestError == null)
                return;

            if (!_scenarioContext.TryGetValue("driver", out IWebDriver driver) || driver == null)
                return;


            var step = _scenarioContext.StepContext.StepInfo;
            var stepText = $"{step.StepDefinitionType} {step.Text}";
            AllureApi.AddAttachment("Failed step", "text/plain", Encoding.UTF8.GetBytes(stepText), ".txt");


            AllureApi.AddAttachment("URL", "text/plain", Encoding.UTF8.GetBytes(driver.Url ?? "N/A"), ".txt");


            if (driver is ITakesScreenshot ts)
            {
                var shot = ts.GetScreenshot();
                AllureApi.AddAttachment("Screenshot", "image/png", shot.AsByteArray, ".png");
            }
        }
    }
}

