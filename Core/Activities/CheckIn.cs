namespace ChadwickSoftware.DeveloperAchievements.Activities
{
    public class CheckIn : Activity
    {
        public virtual string Revision { get; set; }

        protected internal override string CalculateKey()
        {
            return Revision;
        }
    }
}
