using System.Diagnostics;

namespace DeveloperAchievements.Subversion
{
    public static class Console
    {
        private static ServerSideCommitListener _serverSideCommitListener;
        internal static ServerSideCommitListener ServerSideCommitListener
        {
            get { return _serverSideCommitListener; }
            set { _serverSideCommitListener = value; }
        }

        public static void Main(string[] args)
        {
            Debug.Listeners.Add(new ConsoleTraceListener());

            Debug.WriteLine("\nArguments: ", string.Join(", ", args));

            var action = args[0];

            if (action.Contains("commit"))
                ServerSideCommitListener.OnCommit(args[1], args[2]);
        }

    }
}
