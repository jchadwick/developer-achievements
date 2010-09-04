using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ChadwickSoftware.DeveloperAchievements.Website.Services.Contracts
{

    [DataContract]
    public class LogDeveloperActivityResponse
    {
        [DataMember]
        public ActivityResult[] ActivityResults { get; set; }

        public LogDeveloperActivityResponse()
        {
        }

        public LogDeveloperActivityResponse(IEnumerable<ActivityResult> activityResults)
        {
            ActivityResults = activityResults.ToArray();
        }
    }

    [DataContract]
    public class ActivityResult
    {
        [DataMember]
        public string Activity { get; set; }

        [DataMember]
        public string Developer { get; set; }

        [DataMember]
        public int AwardedAchievementCount { get; set; }

        [DataMember]
        public string[] AwardedAchievements { get; set; }
    }

}