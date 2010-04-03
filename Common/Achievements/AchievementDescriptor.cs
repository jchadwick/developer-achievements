namespace DeveloperAchievements.Achievements
{
    public class AchievementDescriptor : KeyedEntity
    {
        public virtual string Name { get; set; }

        protected override string CreateKey()
        {
            return CreateKey(Name);
        }
    }
}