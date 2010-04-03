using System;

namespace DeveloperAchievements.Subversion.InterOp
{
    public class SvnLog
    {
        public string RepositoryPath { get; set; }

        public string Identifier { get; set; }

        public string Author { get; private set; }

        public string Message { get; private set; }

        public DateTime Timestamp { get; private set; }

        public SvnLog(string repositoryPath, string identifier, DateTime timestamp, string author, string message)
        {
            RepositoryPath = repositoryPath;
            Identifier = identifier;
            Author = author;
            Message = message;
            Timestamp = timestamp;
        }
    }
}