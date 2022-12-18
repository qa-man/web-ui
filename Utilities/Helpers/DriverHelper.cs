using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Utilities.Helpers
{
    public static class DriverHelper
    {
        public static WebDriver Driver { get { if (driver is null) SetupBrowser(TestRunHelper.Browser); return driver; } }

        private static WebDriver driver;

        private static List<string> ChromiumArguments => new()
        {
            "-inprivate",
            "--start-maximized",
            "allow-running-insecure-content",
            "test-type",
            "ignore-certificate-errors",
            "disable-extensions",
            "enable-precise-memory-info",
            "js-flags=--expose-gc",
            "--no-sandbox",
            "lang=en-GB"
        };

        public static WebDriver SetupBrowser(Enums.Browser browser)
        {

            var driverManager = new DriverManager();

            switch (browser)
            {
                case Enums.Browser.chrome:
                    driverManager.SetUpDriver(new ChromeConfig());

                    var driverService = ChromeDriverService.CreateDefaultService();
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArguments(ChromiumArguments);
                    chromeOptions.AddExcludedArgument("enable-automation");
                    chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
                    chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);

                    driver = new ChromeDriver(driverService, chromeOptions);

                    break;

                case Enums.Browser.edge:
                    driverManager.SetUpDriver(new EdgeConfig());

                    var edgeDriverService = EdgeDriverService.CreateDefaultService();
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AddArguments(ChromiumArguments);
                    edgeOptions.AddExcludedArgument("enable-automation");
                    edgeOptions.BinaryLocation = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";

                    driver = new EdgeDriver(edgeDriverService, edgeOptions);

                    break;

                default:
                    goto case Enums.Browser.chrome;

            }

            return Driver;
        }

        public static bool WaitForLoading(this WebDriver driver, double timeout = 30, double step = 0.5, bool waitJS = true)
        {
            bool loadingIndicatorAbsent, confirmationListLoadingAbsent, jsCompleted;

            bool WaitForInitialLoading()
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//div[@id='app' and contains(text(), 'Loading')]")));
            }

            bool WaitForConfirmationListLoading()
            {
                return new WebDriverWait(driver, TimeSpan.FromSeconds(timeout)).Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath("//h4[contains(text(), 'Loading Confirmation List')]")));
            }

            bool WaitForJavaScript()
            {
                return driver.ExecuteScript("return document.readyState").Equals("complete");
            }

            Func<bool> resultFunction;

            if (waitJS)
            {
                resultFunction = () =>
                {
                    loadingIndicatorAbsent = confirmationListLoadingAbsent = jsCompleted = false;
                    Parallel.Invoke(() => loadingIndicatorAbsent = WaitForInitialLoading(), () => confirmationListLoadingAbsent = WaitForConfirmationListLoading(), () => jsCompleted = WaitForJavaScript());
                    return loadingIndicatorAbsent && confirmationListLoadingAbsent && jsCompleted;
                };
            }
            else
            {
                resultFunction = () =>
                {
                    loadingIndicatorAbsent = confirmationListLoadingAbsent = false;
                    Parallel.Invoke(() => loadingIndicatorAbsent = WaitForInitialLoading(), () => confirmationListLoadingAbsent = WaitForConfirmationListLoading());
                    return loadingIndicatorAbsent && confirmationListLoadingAbsent;
                };
            }

            return WaitHelper.WaitFor(resultFunction, timeout, step);
        }

        public static void Quit() => driver.Quit();
    }
}