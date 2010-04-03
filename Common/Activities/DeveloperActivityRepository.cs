namespace DeveloperAchievements.Activities
{
    public abstract class DeveloperActivityRepository : IDeveloperActivityRepository
    {
        private readonly IRepository _repository;

        protected internal IRepository Repository
        {
            get { return _repository; }
        }

        protected DeveloperActivityRepository(IRepository repository)
        {
            _repository = repository;
        }

        public void Save(DeveloperActivity activity)
        {
            Repository.Save(activity);
        }
    }
}