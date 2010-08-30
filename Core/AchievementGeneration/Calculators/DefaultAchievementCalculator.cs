using System;
using System.Collections.Generic;
using System.Linq;
using ChadwickSoftware.DeveloperAchievements.DataAccess;

namespace ChadwickSoftware.DeveloperAchievements.AchievementGeneration.Calculators
{
    public class DefaultAchievementCalculator : IAchievementCalculator
    {
        protected IRepository Repository
        {
            get { return _repository; }
        }
        private readonly IRepository _repository;

        public virtual int ExecutionPriority
        {
            get { return 50; }
        }


        public DefaultAchievementCalculator(IRepository repository)
        {
            _repository = repository;
        }


        public virtual IEnumerable<AwardedAchievement> Calculate(Activity trigger)
        {
            List<AwardedAchievement> awardedAchievements = new List<AwardedAchievement>();

            IEnumerable<Achievement> achievements = GetApplicableAchievements(trigger);

            foreach (Achievement achievement in achievements)
            {
                Achievement achievementCopy = achievement;
                AwardedAchievement awardedAchievement =
                    Repository
                        .Query<AwardedAchievement>()
                        .SingleOrDefault(x => x.Achievement == achievementCopy && x.Developer == trigger.Developer);

                if (awardedAchievement == null)
                {
                    awardedAchievement = new AwardedAchievement()
                                             {
                                                 Achievement = achievement,
                                                 Count = 0,
                                                 Developer = trigger.Developer,
                                             };
                }

                awardedAchievement.Count += 1;
                awardedAchievement.LastAwardedTimestamp = DateTime.Now;

                awardedAchievements.Add(awardedAchievement);
            }

            return awardedAchievements;
        }

        protected virtual IEnumerable<Achievement> GetApplicableAchievements(Activity trigger)
        {
            int currentActivityCount = GetCurrentActivityCount(trigger);

            string triggerActivityType = trigger.GetType().FullName;

            IQueryable<Achievement> applicableAchievements =
                Repository.Query<Achievement>()
                    .Where(x => x.TargetActivityTypeName == triggerActivityType)
                    .Where(x => x.TriggerCount == currentActivityCount);

            return applicableAchievements;
        }

        protected virtual int GetCurrentActivityCount(Activity trigger)
        {
            if (trigger == null)
                throw new ApplicationException("Expected to be associated an Activity, but none was specified");

            // Get all previous instances of this type of activity
            int currentCount = trigger.Developer.History.Count(x => x.GetType() == trigger.GetType());

            // The history should already include this trigger, but
            // if not increment the count to take this one into account
            if (!trigger.Developer.History.Any(x => x == trigger))
                currentCount += 1;

            return currentCount;
        }

    }
}