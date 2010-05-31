using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace DeveloperAchievements.Activities
{
    [DataContract(Name="Activity"), KnownType("GetKnownTypes")]
    public abstract class DeveloperActivity : KeyedEntity
    {
        private static readonly object KnownTypesLock = new object();

        private static IEnumerable<Type> _knownTypes;

        [DataMember]
        public virtual string Username { get; set; }

        [IgnoreDataMember]
        public virtual bool Processed { get; set; }


        protected DeveloperActivity()
        {
            CreatedTimeStamp = DateTime.Now;
        }


        // ReSharper disable UnusedMember.Local
        private static IEnumerable<Type> GetKnownTypes()
        {
            if(_knownTypes == null)
            {
                lock(KnownTypesLock)
                {
                    if(_knownTypes == null)
                    {
                        var developerActivityType = typeof(DeveloperActivity);
                        var types = from t in Assembly.GetExecutingAssembly().GetTypes()
                                    where developerActivityType.IsAssignableFrom(t)
                                    select t;
                        _knownTypes = types;
                    }
                }
            }

            return _knownTypes;
        }
        // ReSharper restore UnusedMember.Local
    }
}