using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ChadwickSoftware.DeveloperAchievements.CruiseControl.Proxy;
using ThoughtWorks.CruiseControl.Core;

namespace ChadwickSoftware.DeveloperAchievements.CruiseControl
{
    public class AchievementResult : ITaskResult
    {
        public IEnumerable<ActivityResult> ActivityResults { get; set; }

        public string Data
        {
            get
            {
                IEnumerable<XElement> xmlResults =
                    from result in ActivityResults
                    select
                        new XElement("activity-result",
                            new XElement("activity", result.Activity),
                            new XElement("developer", result.Developer),
                            new XElement("achievement-count", result.AwardedAchievementCount),
                            new XElement("achievements",
                                result.AwardedAchievements.Select(x => 
                                    new XElement("achievement", x))
                            )
                        );

                return new XElement("achievement-results", xmlResults).ToString();
            }
        }


        public AchievementResult()
        {
            ActivityResults = new List<ActivityResult>();
        }

        public AchievementResult(LogDeveloperActivityResponse response)
        {
            ActivityResults = (response == null) 
                ? Enumerable.Empty<ActivityResult>()
                : response.ActivityResults.ToList();
        }


        public bool CheckIfSuccess()
        {
            return true;
        }
    }
}