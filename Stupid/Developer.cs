using FluentNHibernate.Mapping;

namespace Stupid
{
    public class Developer
    {
        public virtual int ID { get; set; }

        public virtual string Username { get; set; }
    }

    internal class DeveloperMapper : ClassMap<Developer>
    {
        public DeveloperMapper()
        {
            Id(x => x.ID);
            Map(x => x.Username);
        }
    }
}