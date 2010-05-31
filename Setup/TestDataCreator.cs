using System.Collections.Generic;
using DeveloperAchievements;
using DeveloperAchievements.Achievements;

namespace Setup
{
    public class TestDataCreator
    {
        private readonly IRepository _repository;

        public TestDataCreator(IRepository repository)
        {
            _repository = repository;
        }

        public void CreateTestData()
        {
            IEnumerable<KeyedEntity> entities = CreateEntities();
            _repository.Save(entities);
        }

        private IEnumerable<KeyedEntity> CreateEntities()
        {
            var jchadwick = new Developer() { Username = "jchadwick", DisplayName = "Jess Chadwick" };

            yield return jchadwick;
            yield return new Developer() { Username = "fsinatra", DisplayName = "Frank Sinatra" };
            yield return new Developer() { Username = "jadama", DisplayName = "Admiral Adama" };

            var bobTheBuilder = new AchievementDescriptor() { Key = "BobTheBuilder", Name = "Bob the Builder" };
            yield return bobTheBuilder;
            yield return new AchievementDescriptor() { Key = "FredTheFailure", Name = "Fred the Failure" };

            yield return new Achievement() { AwardedAchievement = bobTheBuilder, Developer = jchadwick };
        }
    }
}