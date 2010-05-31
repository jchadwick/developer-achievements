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
        [DataMember]
        public virtual string ReportUrl { get; set; }

        [DataMember]
        public virtual BuildResult Result { get; set; }
    }

}