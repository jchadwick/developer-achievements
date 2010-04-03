using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DeveloperAchievements.Subversion.InterOp
{
    public class Svn
    {
        public SvnLog Log(string repositoryPath)
        {
            return Log(new SvnLogParameters(repositoryPath)).SingleOrDefault();
        }

        public IEnumerable<SvnLog> Log(SvnLogParameters parameters)
        {
            string commandLine = string.Format("svn log --xml \"{0}\" --revision={1}",
                parameters.RepositoryPath, parameters.RevisionQuery);

            string output = InterOpHelper.Execute(commandLine);

            return XElement.Parse(output)
                .Descendants("logentry")
                .Select(x => ParseSvnLog(parameters.RepositoryPath, x));
        }

        public SvnInfo Info(string repositoryPath)
        {
            string command = string.Format("svn info --xml " + repositoryPath);

            string output = InterOpHelper.Execute(command);

            return ParseSvnInfo(XElement.Parse(output));
        }

        private static SvnInfo ParseSvnInfo(XElement el)
        {
            string repositoryPath = el.Descendants("url").FirstOrDefault().Value;

            var lastCommitElement = el.Descendants("commit").Single();
            int lastRevision = int.Parse(lastCommitElement.Attribute("revision").Value);
            string lastAuthor = lastCommitElement.Element("author").Value;
            DateTime lastDateTime = DateTime.Parse(lastCommitElement.Element("date").Value);

            return new SvnInfo(repositoryPath, lastRevision, lastAuthor, lastDateTime);
        }

        private static SvnLog ParseSvnLog(string repositoryPath, XElement logElement)
        {
            string revision = logElement.Attribute("revision").Value;
            DateTime timestamp = DateTime.Parse(logElement.Element("date").Value);
            string author = logElement.Element("author").Value;
            string message = logElement.Element("msg").Value;

            return new SvnLog(repositoryPath, revision, timestamp, author, message);
        }
    }
}
