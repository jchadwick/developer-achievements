using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ChadwickSoftware.DeveloperAchievements.Website.Services.Contracts
{
    [DataContract(Name="Activity")]
    public class ActivityContract
    {

        [DataMember]
        public string ActivityType { get; set; }

        [DataMember]
        public Dictionary<string, string> ActivityParameters { get; set; }

        [DataMember]
        public DateTime? Timestamp { get; set; }

        [DataMember]
        public string Username { get; set; }


        public ActivityContract()
        {
            Timestamp = DateTime.Now;
        }

    }

}