using System.Runtime.Serialization;
using DeveloperAchievements.Activities;

namespace DeveloperAchievements.Achievements
{
    public class CheckIn : DeveloperActivity
    {
        [DataMember]
        public virtual string RepositoryPath { get; set; }

        [DataMember]
        public virtual int Revision { get; set; }

        [DataMember]
        public virtual string Message { get; set; }
    }
}
