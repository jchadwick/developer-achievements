using System.Collections.Generic;

namespace ChadwickSoftware.DeveloperAchievements
{
    public class Achievement : Entity
    {
        public virtual IEnumerable<AwardedAchievement> AwardedAchievements { get; set; }

        public virtual string Description { get; set; }

        public virtual AchievementDisposition Disposition { get; set; }

        public virtual AchievementKind Kind { get; set; }

        public virtual string Name { get; set; }

        public virtual string TargetActivityTypeName { get; set; }

        public virtual string LogoUrl { get; set; }

        public virtual string LogoThumbnailUrl
        {
            get { return LogoUrl; }
        }

        public virtual int TriggerCount { get; set; }


        public Achievement()
        {
            AwardedAchievements = new List<AwardedAchievement>();
        }

    }
}