using FluentNHibernate.Mapping;

namespace ChadwickSoftware.DeveloperAchievements.DataAccess.Mappings
{
    public class EntityMap<T> : ClassMap<T> where T : Entity
    {
        public EntityMap()
        {
            Id(x => x.ID)
                .GeneratedBy.Identity();

            Map(x => x.Key, "[Key]")
                .Not.Nullable()
                .Unique();
        }
    }
}
