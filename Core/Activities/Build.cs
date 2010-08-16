﻿namespace ChadwickSoftware.DeveloperAchievements.Activities
{
    public abstract class Build : Activity
    {
        public virtual string Url { get; set; }
    }

    public class BrokenBuild : Build
    {
    }

    public class SuccessfulBuild : Build
    {
    }
}
