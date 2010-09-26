using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace ChadwickSoftware.DeveloperAchievements
{
    public class Achievement : Entity
    {
        protected static string LogoBaseUrl
        {
            get { return ConfigurationManager.AppSettings.Get("Achievements.ImagePath"); }
        }

        public virtual IEnumerable<AwardedAchievement> AwardedAchievements { get; set; }

        public virtual string Description { get; set; }

        public virtual AchievementDisposition Disposition { get; set; }

        public virtual AchievementKind Kind { get; set; }

        public virtual string Name { get; set; }

        public virtual string TargetActivityTypeName { get; set; }

        public virtual string LogoUrl
        {
            get
            {
                if (_logoUrl == null)
                {
                    string baseUrl = ConfigurationManager.AppSettings.Get("Achievements.ImagePath");
                    if (string.IsNullOrEmpty(baseUrl))
                        baseUrl = "/content/images/achievements";

                    string imageName = Key.Replace(' ', '-').Replace("'", string.Empty) + ".png";
                    _logoUrl = Path.Combine(baseUrl, imageName);
                }

                return _logoUrl;
            }
            set { _logoUrl = value; }
        }
        private string _logoUrl;

        public virtual string LogoThumbnailUrl
        {
            get { return _logoThumbnailUrl ?? LogoUrl; }
            set { _logoThumbnailUrl = value; }
        }
        private string _logoThumbnailUrl;

        public virtual int TriggerCount { get; set; }


        public Achievement()
        {
            AwardedAchievements = new List<AwardedAchievement>();
        }

    }
}