using System.Collections.Generic;
using System.Linq;
using DeveloperAchievements.Activities;

namespace DeveloperAchievements.Achievements
{
    public class AchievementService : IAchievementService
    {
        private readonly IAchievementRepository _achievementRepository;
        private readonly IDeveloperRepository _developerRepository;

        private readonly List<IAchievementGenerator> _generators;
        public IList<IAchievementGenerator> Generators
        {
            get { return _generators; }
        }


        public AchievementService(IAchievementRepository achievementRepository, IDeveloperRepository developerRepository)
            : this(achievementRepository, developerRepository, null)
        {
        }
        public AchievementService(IAchievementRepository achievementRepository, IDeveloperRepository developerRepository, IEnumerable<IAchievementGenerator> generators)
        {
            _achievementRepository = achievementRepository;
            _developerRepository = developerRepository;
            _generators = new List<IAchievementGenerator>(generators ?? Enumerable.Empty<IAchievementGenerator>());
        }


        public void Award(string username, AchievementDescriptor achievementDescriptor)
        {
            var user = _developerRepository.GetOrCreate(username);
            Award(user, achievementDescriptor);
        }

        public void Award(Developer developer, AchievementDescriptor achievementDescriptor)
        {
            var award = new Achievement() {Developer = developer, AwardedAchievement = achievementDescriptor};
            _achievementRepository.Save(award);
        }

        public void GenerateAchievements(DeveloperActivity activity)
        {
            foreach (var generator in Generators)
            {
                var generationResult = generator.Generate(activity, null);
                if (generationResult.HasGeneratedAchievements == false)
                    continue;

                foreach (var award in generationResult.GeneratedAchievements)
                {
                    _achievementRepository.Save(award);
                }
            }
        }

        public void RegisterGenerators(params IAchievementGenerator[] generators)
        {
            _generators.AddRange(generators);
        }
    }
}