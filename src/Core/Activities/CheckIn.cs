namespace ChadwickSoftware.DeveloperAchievements.Activities
{
    public class CheckIn : Activity
    {
        public virtual string Revision { get; set; }

        public override string DisplayName
        {
            get { return "Check In"; }
        }

        protected internal override string CalculateKey()
        {
            return Revision;
        }
    }
}
