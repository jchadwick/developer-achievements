using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Autofac;
using Autofac.Builder;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;

namespace DeveloperAchievements.ActivityListener
{
    public class Program
    {
        protected static IContainer Container;
        private static IEnumerable<string> CommandLineArguments;

        static void Main(string[] args)
        {
            Console.WriteLine("Developer Achievements Activity Listener Console Application");

            CommandLineArguments = args;

            if (DebugEnabled)
                Debug.Listeners.Add(new ConsoleTraceListener());

            InitializeIocContainer();


            if (args.Length >0 && Regex.IsMatch(args[0], @"create(Db|Database)", RegexOptions.IgnoreCase))
            {
                Console.WriteLine("Creating database...");
                Container.Resolve<IDataConfiguration>().CreateDatabase();
                return;
            }

            var application = Container.Resolve<Application>();
            Debug.WriteLine("Starting application...");
            application.Start();
        }

        private static bool DebugEnabled
        {
            get
            {
                if(CommandLineArguments.Contains("debug"))
                    return true;

                var debugSetting = ConfigurationManager.AppSettings["debug"];
                if(string.IsNullOrEmpty(debugSetting))
                    return false;
                return Regex.IsMatch(debugSetting, "true|yes|1", RegexOptions.IgnoreCase);
            }
        }

        private static void InitializeIocContainer()
        {
            Debug.WriteLine("Initializing dependencies...");

            var builder = new ContainerBuilder();
            builder.Register<Application>();


            builder.Register<AchievementService>().As<IAchievementService>();
            builder.Register<AchievementRepository>().As<IAchievementRepository>();
            builder.Register<DeveloperRepository>().As<IDeveloperRepository>();

            builder.RegisterModule(new DataAccess.NHibernate.DependencyModule());

            builder
                .RegisterCollection<IAchievementGenerator>()
                .As<IEnumerable<IAchievementGenerator>>();
            builder
                .Register<BuildBreakerAchievementGenerator>()
                .As<IAchievementGenerator>()
                .MemberOf<IEnumerable<IAchievementGenerator>>();


            builder
                .RegisterCollection<IDeveloperActivityListener>()
                .As<IEnumerable<IDeveloperActivityListener>>();
            builder
                .Register<PollingDeveloperActivityListener>()
                .As<IDeveloperActivityListener>()
                .MemberOf<IEnumerable<IDeveloperActivityListener>>();
            
            Container = builder.Build();
        }
    }

    internal class BuildBreakerAchievementGenerator : IAchievementGenerator
    {
        private readonly IRepository _repository;

        public BuildBreakerAchievementGenerator(IRepository repository)
        {
            _repository = repository;
        }

        public AchievementGenerationResult Generate(DeveloperActivity catalyst, DeveloperHistory history)
        {
            var buildBreaker = _repository.FindByKey<AchievementDescriptor>("build+breaker");
            var developer = _repository.FindByKey<Developer>(catalyst.Username);
            return new AchievementGenerationResult()
                       {GeneratedAchievements = new[] {new Achievement {Developer = developer, AwardedAchievement = buildBreaker}}};
        }
    }
}
