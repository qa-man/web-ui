using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;
using Utilities.Models;

namespace Utilities.Helpers
{
    public static class TestDataHelper
    {
        public static List<Dictionary<string, string>> GetTestCaseData(int testCaseId)
        {
            var response = GetTestCaseFields(testCaseId);
            return response.Contains("sharedParameter") ? GetTestDataSharedParametersValues(response) : GetTestCaseParameters(response);
        }

        public static string GetTestCaseParameter(int testCaseId, string key)
        {
            var testCaseParams = GetTestCaseData(testCaseId).Single();
            return testCaseParams.ContainsKey(key) ? testCaseParams[key] : throw new KeyNotFoundException($"Value absent for parameter '{key}'");
        }

        public static List<string> GetTableFromTestCase(Table table)
        {
            return table.Rows.Select(tableRow => tableRow[table.Header.First()]).ToList();
        }

        public static string GetTestCaseAttachment(int testCase)
        {
            var response = GetReqWithAuth($"{ConfigHelper.AzureHost}/{ConfigHelper.AzureCollectionURL}/_apis/wit/workitems?ids={testCase}&$expand=all&api-version=5.0");
            var attachment = JsonConvert.DeserializeObject<TestCaseInfo>(response.Result)?.Value.Single().Relations.Where(rel => rel.Rel.Equals("AttachedFile")).ToList().Single();

            var filePath = $"{Directory.GetCurrentDirectory()}\\{attachment.Attributes.Name}";
            var file = GetAttachmentReqWithAuth($"{ConfigHelper.AzureHost}/{ConfigHelper.AzureCollectionURL}/_apis/wit/attachments/{attachment.Url.LocalPath.Split('/').Last()}?api-version=5.0");
            File.WriteAllBytesAsync(filePath, file.Result);

            return filePath;
        }

        #region private methods

        private static DataTable GetDataTableOfSharedParameters(string xml)
        {
            var resultTable = new DataTable();
            var xElement = XElement.Parse(xml);

            resultTable = GetTableHeaders(xElement, resultTable);
            resultTable = GetTableRows(xElement, resultTable);

            return resultTable;
        }

        private static string GetJsonProperty(string json, string propertyName)
        {
            var jsonObject = JObject.Parse(json);
            var property = jsonObject.Properties().FirstOrDefault(p => p.Name.Equals(propertyName, StringComparison.CurrentCultureIgnoreCase));
            if (property == null)
            {
                throw new Exception($"{propertyName} was not found in json: {json}");
            }

            return Regex.Match(property.Value.ToString(), @"\d+").Value;
        }

        private static List<Dictionary<string, string>> GetParameters(string xml)
        {
            var doc = new XmlDocument();

            doc.LoadXml(xml);
            var paramSets = doc.GetElementsByTagName("Table1");

            return (from XmlNode set in paramSets select set.Cast<XmlNode>().ToDictionary(param => param.Name, param => GetReplacedSpecialCharacters(param.InnerXml))).ToList();
        }

        private static string GetReplacedSpecialCharacters(string value)
        {
            return System.Net.WebUtility.HtmlDecode(value).
                Replace("&amp;#39;", "'").
                Replace("&amp;", "&").
                Replace("&gt;", ">").
                Replace("&lt;", "<").
                Replace("&quot;", "\"");
        }

        private static int GetSharedParameterDataSetId(string json)
        {
            var sharedParameterDataSetId = GetJsonProperty(json, "sharedParameterDataSetIds");
            if (string.IsNullOrWhiteSpace(sharedParameterDataSetId))
            {
                throw new Exception($"no 'sharedParameterDataSetId' property in json: {json}");
            }

            int.TryParse(sharedParameterDataSetId, out var id);

            return id;
        }

        private static DataTable GetTableHeaders(XElement xElement, DataTable resultTable)
        {
            var headers = xElement.Descendants("param");

            foreach (var header in headers)
            {
                resultTable.Columns.Add(header.Value, typeof(string));
            }

            return resultTable;
        }

        private static DataTable GetTableRows(XElement xElement, DataTable resultTable)
        {
            var paramData = xElement.Descendants("paramData").Descendants("dataRow");
            foreach (var dataRows in paramData)
            {
                var row = resultTable.NewRow();
                foreach (var node in dataRows.Elements())
                {
                    var value = node.Attribute("value")?.Value;
                    value = GetReplacedSpecialCharacters(value);
                    var column = node.Attribute("key")?.Value;
                    row[column] = value;
                }

                resultTable.Rows.Add(row);
            }

            return resultTable;
        }

        private static string GetTestCaseFields(int testCase)
        {
            var response = GetReqWithAuth($"https://dev.azure.com/{ConfigHelper.AzureCollectionURL}/_apis/wit/workitems/{testCase}/?fields=Microsoft.VSTS.TCM.LocalDataSource&api-version=1.0").Result;
            try
            {
                var result = JsonConvert.DeserializeObject<WorkItem>(response);
                var fields = result.Fields.LocalDataSource;
                if (fields == null)
                {
                    throw new Exception("Test case was not found with given information.");
                }

                return fields;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to retrieve test case parameters.");
                Console.WriteLine($"Exception - '{e.Message}'.");
                throw new Exception("Please check dev.azure information.");
            }
        }

        private static List<Dictionary<string, string>> GetTestCaseParameters(string fields)
        {
            try
            {
                return GetParameters(fields);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to retrieve test case parameters.");
                Console.WriteLine($"Exception - '{e.Message}'.");
                throw new Exception("Please check dev.azure information.");
            }
        }

        private static List<Dictionary<string, string>> GetTestDataSharedParametersValues(string json)
        {
            var sharedParameterDataSetId = GetSharedParameterDataSetId(json);
            var sharedParamsValues = RequestSharedParameters(sharedParameterDataSetId);
            var sharedParamsResult = JsonConvert.DeserializeObject<WorkItem>(sharedParamsValues);
            var sharedParamsFields = sharedParamsResult.Fields.Parameters;

            var parameters = GetDataTableOfSharedParameters(sharedParamsFields);

            return parameters.AsEnumerable().Select(
                   row => parameters.Columns.Cast<DataColumn>().ToDictionary(
                   column => column.ColumnName,
                   column => row[column].ToString())).ToList();
        }

        private static string RequestSharedParameters(int testCase)
        {
            return GetReqWithAuth($"https://dev.azure.com/{ConfigHelper.AzureCollectionURL}/_apis/wit/workitems/{testCase}/?fields=Microsoft.VSTS.TCM.LocalDataSource&api-version=1.0").Result;
        }

        private static async Task<string> GetReqWithAuth(string request)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{string.Empty}:{TestRunHelper.PAT}")));

                using var httpResponseMessage = httpClient.GetAsync(request).Result;
                httpResponseMessage.EnsureSuccessStatusCode();
                var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

                return responseBody;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return string.Empty;
            }
        }

        private static async Task<byte[]> GetAttachmentReqWithAuth(string request)
        {
            try
            {
                using var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{string.Empty}:{TestRunHelper.PAT}")));

                using var httpResponseMessage = httpClient.GetAsync(request).Result;
                httpResponseMessage.EnsureSuccessStatusCode();
                var responseBody = await httpResponseMessage.Content.ReadAsByteArrayAsync();

                return responseBody;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                return null;
            }
        }

        #endregion

    }
}