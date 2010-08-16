using System.Runtime.Serialization;

namespace ChadwickSoftware.DeveloperAchievements.Website.Services.Contracts
{
    [DataContract]
    public class LogDeveloperActivityResponse
    {
        [DataMember]
        public int AwardedAchievementCount { get; set; }

        [DataMember]
        public long ActivityID { get; set; }
    }
}