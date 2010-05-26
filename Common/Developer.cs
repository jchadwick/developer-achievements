using System.Data.Services.Common;

namespace DeveloperAchievements
{
    [DataServiceEntity]
    public class Developer : KeyedEntity
    {
        public virtual string Username { get; set; }

        protected override string CreateKey()
        {
            return Username;
        }
    }
}