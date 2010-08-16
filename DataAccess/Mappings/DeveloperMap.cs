namespace ChadwickSoftware.DeveloperAchievements.DataAccess.Mappings
{
    public class DeveloperMap : EntityMap<Developer>
    {
        public DeveloperMap()
        {
            HasMany(x => x.Activities)
                .Cascade.All();
            
            HasMany(x => x.Achievements)
                .Cascade.All();
            
            Map(x => x.DisplayName);
            
            HasMany(x => x.History)
                .Cascade.All();
            
            Map(x => x.Username)
                .Not.Nullable();

            Component(x => x.Statistics, stats =>
                                             {
                                                 stats.Map(y => y.LastUpdated).Update();
                                                 stats.Map(y => y.NegativeAchievementsCount);
                                                 stats.Map(y => y.PositiveAchievementsCount);
                                                 stats.Map(y => y.Percentage);
                                                 stats.Map(y => y.Rank);
                                                 stats.Map(y => y.RankLastCalculated);
                                                 stats.Map(y => y.TotalAchievementsCount);
                                                 stats.Map(y => y.TotalNonNeutralAchievementsCount);
                                             }).Update();
        }
    }
}
