using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Diagnostics;
using System.Threading;

namespace Utilities.Helpers
{
    public static class WaitHelper
    {
        static WebDriverWait Wait;

        static WaitHelper()
        {
            Wait = new WebDriverWait(DriverHelper.Driver, TimeSpan.FromSeconds(30)) { PollingInterval = TimeSpan.FromMicroseconds(100), };
            Wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
        }

        public static void WaitUntilVisible(By element) => Wait.Until(ExpectedConditions.ElementIsVisible(element));

        public static void WaitElementPresent(By element) => Wait.Until(driver => driver.FindElement(element));

        public static void WaitElementToBeDisplayed(By element)
        {
            Wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }

        public static bool WaitFor(Func<bool> func, double timeout = 10, double step = 1, string errorMessage = "Wait exceeded time limit.")
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            do
            {
                Thread.Sleep(TimeSpan.FromSeconds(step));
                if (func.Invoke()) return true;
            }
            while (stopwatch.Elapsed.TotalSeconds < timeout);

            return false;
        }
    }
}