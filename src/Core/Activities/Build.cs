namespace ChadwickSoftware.DeveloperAchievements.Activities
{
    public abstract class Build : Activity
    {
        public virtual string Url { get; set; }
    }

    public class FixedBuild : Build
    {
        public override string DisplayName
        {
            get { return "Fixed Build"; }
        }
    }

    public class BrokenBuild : Build
    {
        public override string DisplayName
        {
            get { return "Broken Build"; }
        }
    }

    public class SuccessfulBuild : Build
    {
        public override string DisplayName
        {
            get { return "Successful Build"; }
        }
    }
}
