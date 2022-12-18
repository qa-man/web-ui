using OpenQA.Selenium;
using Tests.Basics;
using Utilities.Helpers;

namespace Tests.Pages
{
    public class AzureLoginPage : BasePage
    {
        private readonly By usernameInput = By.XPath("//input[@type='email']");
        private readonly By passwordInput = By.XPath("//input[@type='password']");
        private readonly By nextButton = By.XPath("//input[@type='submit']");
        private readonly By submitButton = By.XPath("//input[@type='submit']");

        public void Login()
        {
            FillUsername();
            FillPassword();
        }

        #region Private Methods

        private void FillUsername()
        {
            ActionHelper.EnterText(usernameInput, TestRunHelper.Username);
            ActionHelper.ClickElement(nextButton);
        }

        private void FillPassword()
        {
            ActionHelper.EnterText(passwordInput, TestRunHelper.Password);
            ActionHelper.ClickElement(submitButton);
        }

        #endregion
    }
}
