using System;
using System.Windows.Forms;
using DeveloperAchievements.Achievements;
using DeveloperAchievements.Activities;
using DeveloperAchievements.DataAccess.NHibernate;
using DeveloperAchievements.DataAccess.NHibernate.Configuration;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var config = new MsSqlNHibernateConfiguration();
            using(var repository = new Repository(config.CreateSessionFactory().OpenSession()))
            {
                repository.Save(CreateTestActivity());
            }
        }

        private DeveloperActivity CreateTestActivity()
        {
            var activityType = (string)ActivityTypeSelector.SelectedItem;
            var username = UsernameInput.Text;

            switch(activityType)
            {
                case("Check-In"):
                    return new CheckIn()
                    {
                        Username = username,
                        Message = "Test Check-In",
                        RepositoryPath = "svn://www.test.com/svn/trunk"
                    };

                case("Build (broken)"):
                    return new Build()
                    {
                        Username = username,
                        ReportUrl = "http://build.test.com/reports/12345.html",
                        Result = BuildResult.Failed
                    };

                case("Build (successful)"):
                    return new Build()
                    {
                        Username = username,
                        ReportUrl = "http://build.test.com/reports/12345.html",
                        Result = BuildResult.Success
                    };

                default:
                    throw new NotSupportedException(string.Format("Activity AwardedAchievement selection '{0}' is not supported.", activityType));
            }
        }

    }
}
