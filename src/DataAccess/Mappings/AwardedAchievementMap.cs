namespace ChadwickSoftware.DeveloperAchievements.DataAccess.Mappings
{
    public class AwardedAchievementMap : EntityMap<AwardedAchievement>
    {
        public AwardedAchievementMap()
        {
            SelectBeforeUpdate();

            References(x => x.Achievement);
            
            Map(x => x.Count);

            References(x => x.Developer);

            Map(x => x.LastAwardedTimestamp)
                .Default("getdate()");
        }
    }
}
