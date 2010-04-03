using System;

namespace DeveloperAchievements
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly IRepository _repository;

        public DeveloperRepository(IRepository repository)
        {
            _repository = repository;
        }

        public Developer GetOrCreate(string username)
        {
            if(string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username", "The developer's username must not be empty");

            var developer = _repository.FindByKey<Developer>(username);

            if(developer == null)
            {
                developer = new Developer() {Username = username};
                _repository.Save(developer);
            }

            return developer;
        }
    }
}