using DeveloperAchievements.Activities;
using DeveloperAchievements.Subversion.InterOp;

namespace DeveloperAchievements.Subversion
{
    public class ServerSideCommitListener
    {
        private readonly IRepository _repository;
        private readonly SvnLook _svnLooker;

        public ServerSideCommitListener(IRepository repository, SvnLook svnLooker)
        {
            _repository = repository;
            _svnLooker = svnLooker;
        }

        public virtual void OnCommit(string repositoryPath, string identifier)
        {
            var parameters = new SvnRevisionParameters(repositoryPath, identifier);
            var info = _svnLooker.Info(parameters);
            var checkIn = new CheckIn
                              {
                                  RepositoryPath = info.RepositoryPath,
                                  Revision = int.Parse(info.Identifier),
                                  Username = info.Author,
                                  CreatedTimeStamp = info.Timestamp,
                                  Message = info.Message
                              };
            _repository.Save(checkIn);
        }

    }
}