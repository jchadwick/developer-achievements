namespace DeveloperAchievements.DataAccess.NHibernate.Mappings
{
    public class DeveloperMapping : KeyedEntityMapping<Developer>
    {
        public DeveloperMapping()
        {
            Map(x => x.Username);
            Map(x => x.DisplayName);
        }
    }
}
