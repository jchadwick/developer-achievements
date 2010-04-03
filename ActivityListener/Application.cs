using System;
using System.Collections.Generic;
using System.Threading;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;

namespace DeveloperAchievements.ActivityListener
{
    public class Application
    {
        private readonly IEnumerable<IDeveloperActivityListener> _activityListeners;
        private readonly IDeveloperActivityRepository _repository;
        private readonly IAchievementService _achievementService;

        public Application(IEnumerable<IDeveloperActivityListener> activityListeners, 
                           IDeveloperActivityRepository repository, 
                           IAchievementService achievementService)
        {
            _activityListeners = activityListeners;
            _repository = repository;
            _achievementService = achievementService;
        }

        public void Start()
        {
            foreach (var listener in _activityListeners)
            {
                listener.DeveloperActivityDetected += DeveloperActivityDetected;
            }

            while(true)
            {                
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        private void DeveloperActivityDetected(object sender, DeveloperActivityEventArgs e)
        {
            _repository.Save(e.Activity);
            _achievementService.GenerateAchievements(e.Activity);
        }
    }
}