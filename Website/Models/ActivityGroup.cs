using System.Collections.Generic;

namespace ChadwickSoftware.DeveloperAchievements.Website.Models
{
    public class ActivityGroup
    {
        public string Name { get; set; }

        public IEnumerable<KeyValuePair<Developer, int>> ActivityCounts { get; set; }
    }
}