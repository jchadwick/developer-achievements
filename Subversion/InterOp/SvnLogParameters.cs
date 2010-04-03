namespace DeveloperAchievements.Subversion.InterOp
{
    public class SvnLogParameters
    {
        public string RepositoryPath { get; set; }

        public int? ChangesSinceRevision { get; set; }

        public string RevisionQuery
        {
            get
            {
                if (ChangesSinceRevision == null)
                    return "HEAD";
                
                return ChangesSinceRevision.Value + ":HEAD";
            }
        }

        public SvnLogParameters(string repositoryPath)
        {
            RepositoryPath = repositoryPath;
        }
    }
}