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
            return View("DeveloperDetails", developer);
        }

        public ActionResult DeveloperList()
        {
            IEnumerable<Developer> developers = _repository.Query<Developer>();
            return View("DeveloperList", developers);
        }

        public ActionResult LeaderBoard()
        {
            IQueryable<Developer> rankedDevelopers = _repository.Query<Developer>().Where(x => x.Statistics.Rank != null);
            IQueryable<Developer> rockStars = rankedDevelopers.OrderBy(x => x.Statistics.Rank);
            IQueryable<Developer> n00bs = rankedDevelopers.OrderByDescending(x => x.Statistics.Rank);


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
