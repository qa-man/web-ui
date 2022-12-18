using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using ScreenRecorderLib;
using TechTalk.SpecFlow;
using Utilities.Helpers;
using TestContext = NUnit.Framework.TestContext;

namespace Tests.Basics
{
    [Binding]
    public static class BaseTest
    {
        public static WebDriver Driver;
        public static int TestCaseId;

        private static TestContext TestContext;
        private static Recorder VideoRecorder;
        private static string RecordedVideo;

        [BeforeScenario]
        public static void TestInitialize(ScenarioContext scenarioContext)
        {
            TestContext = TestContext.CurrentContext;
            TestCaseId = int.Parse(scenarioContext.ScenarioInfo.Tags.Single(tag => int.TryParse(tag, out TestCaseId)));

            Driver ??= DriverHelper.Driver;

            VideoRecorder = Recorder.CreateRecorder();
            RecordedVideo = Path.Combine(TestContext.TestDirectory, $"{TestCaseId}_{TestContext.Test.Name} [{DateTime.Now:dd MMMM yyyy ± HH·mm·ss}].mp4");
            VideoRecorder.Record(RecordedVideo);
        }

        [AfterScenario]
        public static void TestCleanup()
        {
            TestContext = TestContext.CurrentContext;
            var filePath = Path.Combine(TestContext.TestDirectory, $"{TestCaseId}_{TestContext.Test.Name} [{DateTime.Now:dd MMMM yyyy ± HH·mm·ss}]");

            if (TestContext.Result.Outcome.Status is not TestStatus.Passed)
            {
                var screenshot = $"{filePath}.png";
                DriverHelper.Driver.GetScreenshot().SaveAsFile(screenshot, ScreenshotImageFormat.Png);
                TestContext.AddTestAttachment(screenshot);
            }
            else
            {
                var result = TestContext.Result;
                var currentResult = (TestCaseResult)result.GetType().GetRuntimeFields().SingleOrDefault()?.GetValue(result);
                var output = currentResult?.Output;

                var logPath = $"{filePath}.log";
                File.WriteAllText(logPath, output);
                TestContext.AddTestAttachment(logPath);
            }

            VideoRecorder.Stop();
            TestContext.AddTestAttachment(RecordedVideo);
        }

        [AfterTestRun]
        public static void Finalizer()
        {
            DriverHelper.Quit();
        }

        public static List<string> GetTableFromTestCase(Table table)
        {
            return table.Rows.Select(tableRow => tableRow[table.Header.First()]).ToList();
        }
    }
}