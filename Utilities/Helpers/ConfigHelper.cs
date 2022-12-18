using System.IO;
using Microsoft.Extensions.Configuration;

namespace Utilities.Helpers
{
    public static class ConfigHelper
    {
        public static IConfigurationRoot Config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("config.json").Build();

        public static string AzureCollectionURL => Config["AzureCollectionURL"];
        public static string AzureHost => Config["AzureHost"];
    }
}