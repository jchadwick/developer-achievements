using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ChadwickSoftware.DeveloperAchievements.DataAccess;
using ChadwickSoftware.DeveloperAchievements.Website.Models;

namespace ChadwickSoftware.DeveloperAchievements.Website.Controllers
{
    public class AchievementsController : Controller
    {
        private readonly IRepository _repository;

        public AchievementsController(IRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Achievements()
        {
            IEnumerable<Achievement> achievements = _repository.Query<Achievement>();
            return View(achievements);
        }

        public ActionResult Developers(string key)
        {
            if (string.IsNullOrEmpty(key))
                return DeveloperList();

            Developer developer = _repository.Get<Developer>(key);

            IEnumerable<AwardedAchievement> positiveAchievments =
                developer.Achievements.Where(x => x.Achievement.Disposition == AchievementDisposition.Positive);
            IEnumerable<AwardedAchievement> neutralAchievments =
                developer.Achievements.Where(x => x.Achievement.Disposition == AchievementDisposition.Neutral);
            IEnumerable<AwardedAchievement> negativeAchievments =
                developer.Achievements.Where(x => x.Achievement.Disposition == AchievementDisposition.Negative);

            DeveloperDetails details = new DeveloperDetails
                                           {
                                               Developer = developer,
                                               NegativeAchievementCount = negativeAchievments.Count(),
                                               NegativeAchievements = negativeAchievments,
                                               NeutralAchievementCount = neutralAchievments.Count(),
                                               NeutralAchievements = neutralAchievments,
                                               PositiveAchievementCount = positiveAchievments.Count(),
                                               PositiveAchievements = positiveAchievments,
                                           };

            return View("DeveloperDetails", details);
        }

        public ActionResult DeveloperList()
        {
            IEnumerable<Developer> developers = _repository.Query<Developer>();
            return View("DeveloperList", developers);
        }

        public ActionResult LeaderBoard()
        {
            IQueryable<Developer> rankedDevelopers = _repository.Query<Developer>().Where(x => x.Statistics.Rank != null);
            IQueryable<Developer> rockStars = rankedDevelopers.OrderBy(x => x.Statistics.Rank).Take(5);

            string[] rockStarDeveloperKeys = rockStars.Select(x => x.Key).ToArray();
            IQueryable<Developer> n00bs =
                rankedDevelopers
                    .Where(x => !rockStarDeveloperKeys.Contains(x.Key))
                    .OrderByDescending(x => x.Statistics.Rank);


            // TODO: Implement Busy Bees
            IEnumerable<ActivityGroup> busyBees = new [] { new ActivityGroup() { Name = "CheckIn"} };
            foreach (ActivityGroup activityGroup in busyBees)
            {
                activityGroup.ActivityCounts = Enumerable.Empty<KeyValuePair<Developer,int>>();
            }


            LeaderBoardStatistics statistics = new LeaderBoardStatistics
                                                   {
                                                       RockStars = rockStars.Take(5),
                                                       n00bs = n00bs.Take(5),
                                                       ActivityGroups = busyBees
                                                   };

            return View(statistics);
        }

    }
}
