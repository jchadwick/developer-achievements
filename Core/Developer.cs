using System.Collections.Generic;

namespace ChadwickSoftware.DeveloperAchievements
{
    public class Developer : Entity
    {
        public virtual IList<AwardedAchievement> Achievements { get; private set; }

        public virtual IList<Activity> Activities { get; private set; }

        public virtual string DisplayName
        {
            get { return _displayName ?? Username; }
            set { _displayName = value; }
        }
        private string _displayName;

        public virtual IList<Activity> History { get; private set; }

        public virtual DeveloperStatistics Statistics { get; private set; }

        public virtual string Username { get; set; }


        public Developer()
        {
            Activities = new List<Activity>();
            Achievements = new List<AwardedAchievement>();
            History = new List<Activity>();
            Statistics = new DeveloperStatistics();
        }


        protected internal override string CalculateKey()
        {
            return (Username ?? string.Empty)
                .Replace(" ", "")
                .ToLower();
        }

        public override string ToString()
        {
            return DisplayName;
        }
    }
}