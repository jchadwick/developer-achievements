using System.Data.Services.Common;

namespace DeveloperAchievements
{
    [DataServiceEntity]
    public class Developer : KeyedEntity
    {
        public virtual string Username { get; set; }

        public virtual string DisplayName
        {
            get { return _displayName ?? (_displayName = Username); }
            set { _displayName = value; }
        }
        private string _displayName;

        protected override string CreateKey()
        {
            return Username;
        }
    }
}