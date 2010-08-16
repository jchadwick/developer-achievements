using System;

namespace ChadwickSoftware.DeveloperAchievements
{
    public interface IEntity
    {
        long ID { get; set; }

        string Key { get; set; }

        bool IsNewEntity();
    }

    public abstract class Entity : IEntity, IComparable<IEntity>, IComparable
    {
        public virtual long ID { get; set; }

        public virtual string Key
        {
            get { return _key ?? CalculateKey(); }
            set { _key = value; }
        }
        private string _key;

        protected internal virtual string CalculateKey()
        {
            return Guid.NewGuid().ToString();
        }


        public virtual bool IsNewEntity()
        {
            return ID == default(long);
        }

        public virtual int CompareTo(IEntity other)
        {
            if (other == null || GetType() != other.GetType())
                return 0;

            if (string.IsNullOrEmpty(Key))
                return 1;
            else
                return string.Compare(Key, other.Key);
        }

        public virtual int CompareTo(object obj)
        {
            return CompareTo(obj as IEntity);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Entity)) return false;
            return Equals((Entity) obj);
        }

        public virtual bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.ID == ID || Equals(other.Key, Key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ID.GetHashCode()*397) ^ (Key != null ? Key.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return Key;
        }
    }
}