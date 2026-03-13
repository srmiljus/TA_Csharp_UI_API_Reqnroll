using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace UI_Automation.Pages
{
    public class StorePage
    {
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _jsExecutor;
        private readonly WebDriverWait _wait;

        #region Locators (By)

        private readonly By SearchBoxInput = By.XPath("//input[@class='_2tlUAG6WNyYFlk9caIiLj5']");
        private readonly By SearchResultsRows = By.CssSelector(".search_result_row");
        private readonly By GameNameHeading = By.Id("appHubAppName");
        private readonly By PlayGameButton = By.Id("freeGameBtn");
        private readonly By NoINeedSteamButton = By.XPath("//h3[contains(text(),'No, I need Steam')]");

        #endregion

        public StorePage(IWebDriver driver)
        {
            _driver = driver;
            _jsExecutor = (IJavaScriptExecutor)driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }

        #region Helpers

        private IWebElement WaitVisible(By by)
            => _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));

        private IWebElement WaitClickable(By by)
            => _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));

        private IReadOnlyCollection<IWebElement> WaitForSearchResults(int minCount = 1)
        {
            _wait.Until(d =>
            {
                var els = d.FindElements(SearchResultsRows);
                return els != null && els.Count >= minCount;
            });

            return _driver.FindElements(SearchResultsRows);
        }

        private void ScrollTo(By by, int yShift = 350)
        {
            var el = WaitVisible(by);
            var linkYPositionShift = el.Location.Y - yShift;
            _jsExecutor.ExecuteScript("window.scrollBy(0, arguments[0]);", linkYPositionShift);
        }

        #endregion

        #region Actions

        public void SearchForGame(string gameName)
        {
            var search = WaitVisible(SearchBoxInput);
            search.Clear();
            search.SendKeys(gameName);
            search.SendKeys(Keys.Enter);

            // wait that results load (prevents "Sequence contains no elements")
            WaitForSearchResults(1);
        }

        public string GetFirstSearchResultText()
        {
            var results = WaitForSearchResults(1);
            return results.First().Text;
        }

        public string GetSecondSearchResultText()
        {
            var results = WaitForSearchResults(2);
            return results.Skip(1).First().Text;
        }

        public void ClickFirstSearchResult()
        {
            var results = WaitForSearchResults(1);
            results.First().Click();
        }

        public void ClickFirstSearchResultWithJs()
        {
            var results = WaitForSearchResults(1);
            var first = results.First();
            _jsExecutor.ExecuteScript("arguments[0].click();", first);
        }

        public void WaitForGameDetailsPage()
        {
            // Steam page sometimes loads slower in Grid/headless
            WaitVisible(GameNameHeading);
        }

        public string GetPageUrl() => _driver.Url;

        public string GetGameNameHeadingText()
        {
            WaitForGameDetailsPage();
            return WaitVisible(GameNameHeading).Text;
        }

        public void ClickPlayGameButton()
        {
            ScrollTo(PlayGameButton);
            WaitClickable(PlayGameButton).Click();
        }

        public void ClickNoINeedSteamButton()
        {
            // Dismiss any browser-level protocol handler dialog
            try { _driver.SwitchTo().Alert().Dismiss(); } catch { }

            var button = WaitClickable(NoINeedSteamButton);
            _jsExecutor.ExecuteScript("arguments[0].scrollIntoView({block:'center'});", button);
            _jsExecutor.ExecuteScript("arguments[0].click();", button);
        }

        #endregion
    }
}
