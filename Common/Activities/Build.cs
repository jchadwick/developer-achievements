using System.Data.Services.Common;
using System.Runtime.Serialization;

namespace DeveloperAchievements.Activities
{
    [DataContract]
    public enum BuildResult
    {
        
        [EnumMember(Value = "Failed")]
        Failed = -1,

        [EnumMember(Value = "Success")]
        Success = 0,

        [EnumMember(Value = "Fixed")]
        Fixed = 1

    }

    [DataContract]
    public class Build : DeveloperActivity
    {
        public virtual string ReportUrl { get; set; }

        public virtual BuildResult Result { get; set; }
    }

}