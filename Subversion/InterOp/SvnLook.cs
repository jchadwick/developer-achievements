using System;
using System.Diagnostics;
using System.IO;

namespace DeveloperAchievements.Subversion.InterOp
{
    public class SvnLook
    {
        public virtual SvnLog Info(SvnRevisionParameters parameters)
        {
            string author, timestampString, logMessage;

            var commandLine = string.Format("svnlook info \"{0}\" --{1}={2}", 
                parameters.RepositoryPath, parameters.IdentifierType, parameters.Identifier);
            
            string output = InterOpHelper.Execute(commandLine);
            Debug.WriteLine("svnlook info output:\n" + output);
            
            using (var reader = new StringReader(output))
            {
                author = reader.ReadLine();
                timestampString = reader.ReadLine();
                logMessage = reader.ReadToEnd().Trim();
            }

            var timestamp = DateTime.Parse(timestampString.Split('(')[0]);

            var info = new SvnLog(parameters.RepositoryPath, parameters.Identifier, timestamp, author, logMessage);
            Debug.WriteLine("Author: " + info.Author);
            Debug.WriteLine("Timestamp: " + info.Timestamp);
            Debug.WriteLine("Log Message:\n" + info.Message);

            return info;
        }

    }
}