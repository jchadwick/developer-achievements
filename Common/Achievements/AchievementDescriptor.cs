
using System.Runtime.Serialization;

namespace DeveloperAchievements.Achievements
{
    [DataContract(Name="Achievement")]
    public class AchievementDescriptor : KeyedEntity
    {
        [DataMember]
        public virtual string Name { get; set; }

        [DataMember]
        public virtual string Description { get; set; }

        protected override string CreateKey()
        {
            return CreateKey(Name);
        }
    }
}