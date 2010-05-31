using System.IO;
using DeveloperAchievements.Util;
using Ninject;

namespace Setup
{
    class Program
    {
        private static void Main(string[] args)
        {
            KernelFactory factory = new KernelFactory();
            IKernel kernel = factory.GetKernel(Directory.GetCurrentDirectory());
            kernel.Get<DatabaseBuilder>().DropAndCreateDatabase();
            kernel.Get<TestDataCreator>().CreateTestData();
        }
    }
}
