using NUnit.Framework;
using Utilities.Enums;

namespace Utilities.Helpers
{
    public static class TestRunHelper
    {
        public static Environment Environment => (Environment)System.Enum.Parse(typeof(Environment), $"{TestContext.Parameters["environment"]?.ToLower()}");
        public static Browser Browser => (Browser)System.Enum.Parse(typeof(Browser), $"{TestContext.Parameters["browser"]?.ToLower()}");

        public static string PAT => $"{TestContext.Parameters["pat"]}";
        public static string Username => $"{TestContext.Parameters["user"]}";
        public static string Password => $"{TestContext.Parameters["password"]}";
    }
}