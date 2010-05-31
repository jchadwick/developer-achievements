namespace DeveloperAchievements.DataAccess.NHibernate.Mappings
{
    public class UserMapping : KeyedEntityMapping<User>
    {
        public UserMapping()
        {
            Map(x => x.Username);
            Map(x => x.DisplayName);
        }
    }
}
