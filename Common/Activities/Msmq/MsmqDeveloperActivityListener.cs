using System;
using System.Diagnostics;
using System.Messaging;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;
using DeveloperAchievements.Activities.Msmq;

namespace DeveloperAchievements.Achievements.Msmq
{
    public class MsmqDeveloperActivityListener : DeveloperActivityListenerBase, IDisposable
    {
        private readonly IDeveloperActivityRepository _repository;
        private readonly MsmqDeveloperActivitySerializer _serializer;
        private MessageQueue _queue;
        private bool _isStopped = true;
        private IAsyncResult _currentRetrieveRequest;

        public MsmqDeveloperActivityListener(IAchievementService achievementService, IDeveloperActivityRepository repository)
            : this(achievementService, repository, new MsmqDeveloperActivitySerializer())
        {
        }

        public MsmqDeveloperActivityListener(IAchievementService achievementService, IDeveloperActivityRepository repository, MsmqDeveloperActivitySerializer serializer)
            : base(achievementService)
        {
            _repository = repository;
            _serializer = serializer;
        }

        public override void Start()
        {
            _isStopped = false;
            _queue = MsmqConfiguration.CreateQueue();
            _queue.ReceiveCompleted += ReceivedMessage;
            BeginReceive();
        }

        void ReceivedMessage(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                var activity = _serializer.Deserialize(e.Message);

                activity.Processed = true;

                // Save for posterity's sake...
                if(_repository != null)
                    _repository.Save(activity);

                TriggerAchievementGeneration(activity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception generating Achievement: {0}.  Requeuing message...", ex.Message);
                e.Message.DestinationQueue.Send(e.Message);
            }

            try
            {
                BeginReceive();  // Keep on listening!
            }
            catch (MessageQueueException)
            {
                Debug.WriteLine("Received MessageQueueException - ignoring.");
                // Do nothing - this is what happens when the queue is killed (on Stop())
            }
        }

        private void BeginReceive()
        {
            if (!_isStopped)
            {
                _currentRetrieveRequest = _queue.BeginReceive();
            }
        }

        public override void Stop()
        {
            _isStopped = true;

            if (_currentRetrieveRequest != null)
                _currentRetrieveRequest.AsyncWaitHandle.WaitOne(1, true);

            _queue.Dispose();
        }

        public void Dispose()
        {
            if (_queue != null)
                _queue.Dispose();
        }
    }
}