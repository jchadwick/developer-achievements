﻿using System;

namespace ChadwickSoftware.DeveloperAchievements
{
    public abstract class Activity : Entity
    {
        public virtual Developer Developer { get; set; }

        public virtual DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
        private DateTime _timestamp = DateTime.Now;

        public abstract string DisplayName { get; }

        public virtual int CompareTo(Activity other)
        {
            if (other == null)
                return 1;

            return Timestamp.CompareTo(other.Timestamp);
        }

        public override int CompareTo(object obj)
        {
            return CompareTo(obj as Activity);
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}