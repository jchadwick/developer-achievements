using System;

namespace DeveloperAchievements.Subversion.InterOp
{
    public class SvnInfo
    {
        public string RepositoryPath { get; private set; }

        public int LastRevision { get; private set; }

        public string LastAuthor { get; private set; }

        public DateTime LastCommitTimeStamp { get; private set; }


        public SvnInfo(string repositoryPath, int lastRevision, string lastAuthor, DateTime lastCommitTimeStamp)
        {
            RepositoryPath = repositoryPath;
            LastRevision = lastRevision;
            LastAuthor = lastAuthor;
            LastCommitTimeStamp = lastCommitTimeStamp;
        }
    }
}