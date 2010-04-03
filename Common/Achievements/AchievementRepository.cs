namespace DeveloperAchievements.Achievements
{
    public class AchievementRepository : IAchievementRepository
    {
        private readonly IRepository _repository;

        public AchievementRepository(IRepository repository)
        {
            _repository = repository;
        }

        public void Save(Achievement achievement)
        {
            _repository.Save(achievement);
        }
    }
}