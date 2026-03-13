using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace UI_Automation.Pages
{
    public class AboutPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        #region Locators

        private readonly By InstallSteamButtonLocator = By.CssSelector("#about_greeting .about_install_steam_link");
        private readonly By OnlineStatusLocator = By.XPath("//div[contains(@class,'gamers_online')]/parent::div");
        private readonly By PlayingNowStatusLocator = By.XPath("//div[contains(@class,'gamers_in_game')]/parent::div");

        #endregion
        public AboutPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
        }


        public bool IsInstallSteamButtonClickable()
        {
            var button = _wait.Until(ExpectedConditions.ElementToBeClickable(InstallSteamButtonLocator));
            return button.Displayed && button.Enabled;
        }

        public bool CompareIfPlayingNowStatusIsLessThanOnlineStatus()
        {
            var js = (IJavaScriptExecutor)_driver;

            // Scroll to the bottom so the stats section loads into view
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

            // Wait for both stat elements to appear and have non-empty text
            _wait.Until(d =>
            {
                var online = d.FindElements(OnlineStatusLocator);
                var playing = d.FindElements(PlayingNowStatusLocator);
                return online.Count > 0 && playing.Count > 0
                    && !string.IsNullOrWhiteSpace(online.Last().Text)
                    && !string.IsNullOrWhiteSpace(playing.Last().Text);
            });

            var onlineElements = _driver.FindElements(OnlineStatusLocator);
            var playingNowElements = _driver.FindElements(PlayingNowStatusLocator);

            int onlineStatus = int.Parse(onlineElements.Last().Text.Replace("ONLINE", "").Replace(",", "").Trim());
            int playingNowStatus = int.Parse(playingNowElements.Last().Text.Replace("PLAYING NOW", "").Replace(",", "").Trim());

            return onlineStatus > playingNowStatus;
        }
    }

}
