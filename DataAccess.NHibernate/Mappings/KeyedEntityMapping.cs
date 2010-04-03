using FluentNHibernate.Mapping;

namespace DeveloperAchievements.DataAccess.NHibernate.Mappings
{
    public class KeyedEntityMapping<T> : ClassMap<T>
        where T : KeyedEntity
    {
        public KeyedEntityMapping()
        {
            Id(x => x.Id, "Id").GeneratedBy.Native();
            Map(x => x.Key, "[Key]")
                .Not.Nullable()
                .Unique();
            Map(x => x.CreatedTimeStamp, "Created");
        }
    }
}