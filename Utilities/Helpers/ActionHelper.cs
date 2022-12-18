using OpenQA.Selenium;

namespace Utilities.Helpers
{
    public static class ActionHelper
    {
        public static void ClickElement(By element)
        {
            WaitHelper.WaitElementToBeDisplayed(element);
            DriverHelper.Driver.FindElement(element).Click();
        }

        public static void EnterText(By element, string text)
        {
            WaitHelper.WaitElementPresent(element);
            ClickElement(element);
            DriverHelper.Driver.FindElement(element).SendKeys(text);
        }

        public static void ClickElementUsingJs(By element)
        {
            DriverHelper.Driver.ExecuteScript("arguments[0].click();", DriverHelper.Driver.FindElement(element));
        }
    }
}