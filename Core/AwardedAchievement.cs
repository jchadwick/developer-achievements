using System;

namespace ChadwickSoftware.DeveloperAchievements
{
    public class AwardedAchievement : Entity
    {
        public virtual Achievement Achievement { get; set; }

        public virtual int Count { get; set; }

        public virtual Developer Developer { get; set; }

        public virtual DateTime LastAwardedTimestamp { get; set; }


        public AwardedAchievement()
        {
            LastAwardedTimestamp = DateTime.Now;
        }
    }
}