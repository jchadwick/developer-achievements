using System;

namespace ChadwickSoftware.DeveloperAchievements
{
    public class CoverageExcludeAttribute : Attribute
    {
        public string Message { get; set; }

        public CoverageExcludeAttribute(string message)
        {
            Message = message;
        }
    }
}
