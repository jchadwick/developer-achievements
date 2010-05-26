using System.Collections.Generic;
using System.Data.Services.Common;

namespace DeveloperAchievements.Activities
{
    [DataServiceEntity]
    public class DeveloperHistory : KeyedEntity
    {
        public virtual Developer Developer { get; protected internal set; }

        public virtual int TotalBuilds { get; protected internal set; }

        public virtual int TotalCheckins { get; protected internal set; }

        public virtual int TotalTaskActivities { get; protected internal set; }

//        public virtual IEnumerable<DeveloperActivity> PreviousActivity { get; protected internal set; }

        protected override string CreateKey()
        {
            return Developer.Key;
        }
    }
}
