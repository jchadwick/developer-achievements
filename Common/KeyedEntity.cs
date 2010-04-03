using System;
using System.Runtime.Serialization;

namespace DeveloperAchievements
{
    [DataContract, Serializable]
    public abstract class KeyedEntity
    {
        [DataMember]
        protected internal virtual long Id { get; private set; }

        private string _key;
        [DataMember]
        public virtual string Key
        {
            get
            {
                if (_key == null)
                    _key = CreateKey();
                return _key;
            }
            set { _key = value; }
        }

        private DateTime? _createdTimeStamp;
        [DataMember]
        public virtual DateTime CreatedTimeStamp
        {
            get
            {
                if (_createdTimeStamp == null)
                    _createdTimeStamp = DateTime.Now;
                return _createdTimeStamp.Value;
            }
            set { _createdTimeStamp = value; }
        }


        public override bool Equals(object obj)
        {
            return Equals(obj as KeyedEntity);
        }

        public virtual bool Equals(KeyedEntity obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if(Id != default(int) && obj.Id != default(int)) return Id == obj.Id;
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override string ToString()
        {
            return Key;
        }

        protected virtual string CreateKey()
        {
            return CreatedTimeStamp.ToString("yyyyMMddHHmmssfff");
        }

        protected static string CreateKey(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
                throw new ArgumentOutOfRangeException("inputText", "Cannot create a null or empty Entity Key.");

            return System.Web.HttpUtility.UrlEncode(inputText).ToLower();
        }
    }
}