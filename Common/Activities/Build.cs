using System.Data.Services;
using System.Data.Services.Common;
using System.Runtime.Serialization;

namespace DeveloperAchievements.Activities
{
    [DataContract(Name = "BuildResult")]
    public enum BuildResult : short
    {
        [EnumMember(Value = "Failed")]
        Failed = -1,

        [EnumMember(Value = "Unknown")]
        Unknown = 0,

        [EnumMember(Value = "Success")]
        Success = 1,

        [EnumMember(Value = "Fixed")]
        Fixed = 2
    }

    [DataServiceEntity]
    [IgnoreProperties("Result")]
    public class Build : DeveloperActivity
    {
        public virtual string ReportUrl { get; set; }
        
        public virtual BuildResult Result { get; set; }
    }

}