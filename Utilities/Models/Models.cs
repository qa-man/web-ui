using System;
using Newtonsoft.Json;

namespace Utilities.Models
{
    internal class WorkItem
    {
        public Fields Fields { get; set; }
    }

    public class Fields
    {
        [JsonProperty("Microsoft.VSTS.TCM.LocalDataSource")]
        public string LocalDataSource { get; set; }

        [JsonProperty("Microsoft.VSTS.TCM.Parameters")]
        public string Parameters { get; set; }
    }

    public class TestCaseInfo
    {
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("value")]
        public Value[] Value { get; set; }
    }

    public class Value
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("rev")]
        public long Rev { get; set; }

        [JsonProperty("fields")]
        public ValueFields Fields { get; set; }

        [JsonProperty("relations")]
        public Relation[] Relations { get; set; }

        [JsonProperty("_links")]
        public ValueLinks Links { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public class ValueFields
    {
        [JsonProperty("System.Id")]
        public long SystemId { get; set; }

        [JsonProperty("System.AreaId")]
        public long SystemAreaId { get; set; }

        [JsonProperty("System.AreaPath")]
        public string SystemAreaPath { get; set; }

        [JsonProperty("System.TeamProject")]
        public string SystemTeamProject { get; set; }

        [JsonProperty("System.NodeName")]
        public string SystemNodeName { get; set; }

        [JsonProperty("System.AreaLevel1")]
        public string SystemAreaLevel1 { get; set; }

        [JsonProperty("System.Rev")]
        public long SystemRev { get; set; }

        [JsonProperty("System.AuthorizedDate")]
        public DateTimeOffset SystemAuthorizedDate { get; set; }

        [JsonProperty("System.RevisedDate")]
        public DateTimeOffset SystemRevisedDate { get; set; }

        [JsonProperty("System.IterationId")]
        public long SystemIterationId { get; set; }

        [JsonProperty("System.IterationPath")]
        public string SystemIterationPath { get; set; }

        [JsonProperty("System.IterationLevel1")]
        public string SystemIterationLevel1 { get; set; }

        [JsonProperty("System.WorkItemType")]
        public string SystemWorkItemType { get; set; }

        [JsonProperty("System.State")]
        public string SystemState { get; set; }

        [JsonProperty("System.Reason")]
        public string SystemReason { get; set; }

        [JsonProperty("System.AssignedTo")]
        public MicrosoftVstsCommonActivatedBy SystemAssignedTo { get; set; }

        [JsonProperty("System.CreatedDate")]
        public DateTimeOffset SystemCreatedDate { get; set; }

        [JsonProperty("System.CreatedBy")]
        public MicrosoftVstsCommonActivatedBy SystemCreatedBy { get; set; }

        [JsonProperty("System.ChangedDate")]
        public DateTimeOffset SystemChangedDate { get; set; }

        [JsonProperty("System.ChangedBy")]
        public MicrosoftVstsCommonActivatedBy SystemChangedBy { get; set; }

        [JsonProperty("System.AuthorizedAs")]
        public MicrosoftVstsCommonActivatedBy SystemAuthorizedAs { get; set; }

        [JsonProperty("System.PersonId")]
        public long SystemPersonId { get; set; }

        [JsonProperty("System.Watermark")]
        public long SystemWatermark { get; set; }

        [JsonProperty("System.CommentCount")]
        public long SystemCommentCount { get; set; }

        [JsonProperty("System.Title")]
        public string SystemTitle { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.StateChangeDate")]
        public DateTimeOffset MicrosoftVstsCommonStateChangeDate { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.ActivatedDate")]
        public DateTimeOffset MicrosoftVstsCommonActivatedDate { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.ActivatedBy")]
        public MicrosoftVstsCommonActivatedBy MicrosoftVstsCommonActivatedBy { get; set; }

        [JsonProperty("Microsoft.VSTS.Common.Priority")]
        public long MicrosoftVstsCommonPriority { get; set; }

        [JsonProperty("Microsoft.VSTS.TCM.AutomatedTestName")]
        public string MicrosoftVstsTcmAutomatedTestName { get; set; }

        [JsonProperty("Microsoft.VSTS.TCM.AutomatedTestStorage")]
        public string MicrosoftVstsTcmAutomatedTestStorage { get; set; }

        [JsonProperty("Microsoft.VSTS.TCM.AutomatedTestId")]
        public Guid MicrosoftVstsTcmAutomatedTestId { get; set; }

        [JsonProperty("Microsoft.VSTS.TCM.AutomationStatus")]
        public string MicrosoftVstsTcmAutomationStatus { get; set; }

        [JsonProperty("Custom.Workstream")]
        public string CustomWorkstream { get; set; }

        [JsonProperty("Custom.Automation")]
        public string CustomAutomation { get; set; }

        [JsonProperty("Microsoft.VSTS.TCM.Steps")]
        public string MicrosoftVstsTcmSteps { get; set; }

        [JsonProperty("Microsoft.VSTS.TCM.Parameters")]
        public string MicrosoftVstsTcmParameters { get; set; }

        [JsonProperty("Microsoft.VSTS.TCM.LocalDataSource")]
        public string MicrosoftVstsTcmLocalDataSource { get; set; }

        [JsonProperty("System.Tags")]
        public string SystemTags { get; set; }
    }

    public class MicrosoftVstsCommonActivatedBy
    {
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("_links")]
        public MicrosoftVstsCommonActivatedByLinks Links { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("uniqueName")]
        public string UniqueName { get; set; }

        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("descriptor")]
        public string Descriptor { get; set; }
    }

    public class MicrosoftVstsCommonActivatedByLinks
    {
        [JsonProperty("avatar")]
        public HtmlClass Avatar { get; set; }
    }

    public class HtmlClass
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }
    }

    public class ValueLinks
    {
        [JsonProperty("self")]
        public HtmlClass Self { get; set; }

        [JsonProperty("workItemUpdates")]
        public HtmlClass WorkItemUpdates { get; set; }

        [JsonProperty("workItemRevisions")]
        public HtmlClass WorkItemRevisions { get; set; }

        [JsonProperty("workItemComments")]
        public HtmlClass WorkItemComments { get; set; }

        [JsonProperty("html")]
        public HtmlClass Html { get; set; }

        [JsonProperty("workItemType")]
        public HtmlClass WorkItemType { get; set; }

        [JsonProperty("fields")]
        public HtmlClass Fields { get; set; }
    }

    public class Relation
    {
        [JsonProperty("rel")]
        public string Rel { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }

    public class Attributes
    {
        [JsonProperty("isLocked")]
        public bool IsLocked { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}