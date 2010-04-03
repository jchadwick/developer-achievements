using System;
using System.Windows.Forms;
using DeveloperAchievements.Subversion.InterOp;

namespace TestApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var svn = new Svn();
            var current = svn.Info("http://jchad.mediafreak.org/svn");
            var history = svn.Log(new SvnLogParameters("http://jchad.mediafreak.org/svn") { ChangesSinceRevision = 240});

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
