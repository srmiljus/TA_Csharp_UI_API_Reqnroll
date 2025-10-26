using OpenQA.Selenium;

namespace UI_Automation.Pages
{
    public class StorePage
    {
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _jsExecutor;

        // Locators
        private IWebElement SearchBox => _driver.FindElement(By.XPath("//input[@class='_2tlUAG6WNyYFlk9caIiLj5']"));
        private IList<IWebElement> SearchResults => _driver.FindElements(By.CssSelector(".search_result_row"));
        private IWebElement GameNameHeading => _driver.FindElement(By.Id("appHubAppName"));
        private IWebElement PlayGameButton => _driver.FindElement(By.Id("freeGameBtn"));
        private IWebElement NoINeedSteamButton => _driver.FindElement(By.XPath("//h3[contains(text(),'No, I need Steam')]"));


        public StorePage(IWebDriver driver)
        {
            _driver = driver;
            _jsExecutor = (IJavaScriptExecutor)driver;
        }

        // Actions

        public void SearchForGame(string gameName)
        {
            SearchBox.Clear();
            SearchBox.SendKeys(gameName);
            SearchBox.SendKeys(Keys.Enter);
        }

        public string GetFirstSearchResultText()
        {
            return SearchResults.First().Text;
        }

        public string GetSecondSearchResultText()
        {
            return SearchResults.Count > 1 ? SearchResults[1].Text : string.Empty;
        }

        public void ClickFirstSearchResultWithJs()
        {
            var firstResult = SearchResults.FirstOrDefault();
            if (firstResult != null)
            {
                _jsExecutor.ExecuteScript("arguments[0].click();", firstResult);
            }
        }

        public string GetPageUrl()
        {
            return _driver.Url;
        }

        public string GetGameNameHeadingText()
        {
            return GameNameHeading.Text;
        }

        public void ClickPlayGameButton()
        {
            ScrollToElement(PlayGameButton);
            PlayGameButton.Click();
        }

        public void ClickNoINeedSteamButton()
        {
            NoINeedSteamButton.Click();
        }

        private void ScrollToElement(IWebElement element)
        {
            var linkYPositionShift = element.Location.Y - 350;
            _jsExecutor.ExecuteScript("window.scrollBy(0," + linkYPositionShift + ");");
        }
    }
}
