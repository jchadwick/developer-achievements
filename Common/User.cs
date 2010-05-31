using System.Runtime.Serialization;

namespace DeveloperAchievements
{
    [DataContract]
    public class User : KeyedEntity
    {
        [DataMember]
        public virtual string Username { get; set; }

        [DataMember]
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