namespace DeveloperAchievements.Subversion.InterOp
{
    public class SvnRevisionParameters
    {
        public string RepositoryPath { get; set; }

        public string Identifier { get; set; }

        public string IdentifierType
        {
            get { return Identifier.Contains("-") ? "transaction" : "revision"; }
        }

        public SvnRevisionParameters(string repositoryPath, string identifier)
        {
            RepositoryPath = repositoryPath;
            Identifier = identifier;
        }
    }
}