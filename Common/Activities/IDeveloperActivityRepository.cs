using System.Collections.Generic;

namespace DeveloperAchievements.Activities
{
    public interface IDeveloperActivityRepository
    {
        void Save(DeveloperActivity activity);
    }

    public static class IDeveloperActivityRepositoryExtensions
    {
        // TODO: This is backwards...  Save(one) should be an extension of Save(Many[])
        public static void Save(this IDeveloperActivityRepository repository, IEnumerable<DeveloperActivity> buildEvents)
        {
            foreach (var buildEvent in buildEvents)
            {
                repository.Save(buildEvent);
            }
        }

    }
}