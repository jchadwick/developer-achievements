namespace ChadwickSoftware.DeveloperAchievements.DataAccess.Mappings
{
    public class AchievementMap : EntityMap<Achievement>
    {
        public AchievementMap()
        {
            Map(x => x.Description)
                .Length(int.MaxValue);

            Map(x => x.Disposition)
                .Not.Nullable();

            Map(x => x.Kind)
                .Not.Nullable();

            Map(x => x.Name)
                .Not.Nullable();

            Map(x => x.TargetActivityTypeName);

            Map(x => x.TriggerCount);

            HasMany(x => x.AwardedAchievements);
        }
    }
}
