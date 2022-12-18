using OpenQA.Selenium;
using Utilities.Helpers;

namespace Tests.Basics
{
    public class BasePage
    {
        protected WebDriver Driver;

        public BasePage()
        {
            Driver = DriverHelper.Driver;
        }
    }
}
