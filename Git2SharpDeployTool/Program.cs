using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using deploytool.services.Implementation;

namespace Git2SharpDeployTool
{
    
    /*
    * Ref: http://www.woodwardweb.com/git/getting_started_2.html
    * 
    */
    class Program
    {
        //private static UsernamePasswordCredentials Credentials => new UsernamePasswordCredentials()
        //{
        //    Username = "joe.bolla@wpnc.onmicrosoft.com",
        //    Password = "Bealice2000"
        //};
        private static UsernamePasswordCredentials Credentials => new UsernamePasswordCredentials()
        {
            Username = "admin@cyberminds.co.uk",
            Password = "steelpan60"
        };
        private static RepositoryService Service
        {
            get { return new RepositoryService(); }
        }

        static void Main(string[] args)
        {
            //GetReposDetails();
            //GetTipOfHead();
            //ListContentOfCommit();

            var targetDir = @"C:\Projects\Tutorials\GitLibraries\test";
            //var reposUrl = @"https://wpnc.visualstudio.com/DefaultCollection/LogicGroup-Website/_git/code-lg-reporting";           
            var reposUrl = @"https://github.com/Cmadmin/Git2SharpDeployTool.git";




            //var result = Service.PullOrCloneRepository(targetDir, reposUrl, Credentials, "joe-branch");
            var result = Service.PullOrCloneRepository(targetDir, reposUrl, Credentials, "master");
            Console.WriteLine("Success: " + result.IsSuccess);
            Console.WriteLine(result.ErrorMessage);
            Console.WriteLine(result.Logs);

            Console.ReadKey();

            //branch list
            ListBranches(targetDir);

            Console.ReadKey();
        }

        private static void GetReposDetails()
        {
            using (var repo = new Repository(@"C:\Projects\Tutorials\TotalHR"))
            {
                Commit commit = repo.Lookup<Commit>("4ca8d31844d153b64192fdc6bd8f54c1fc888195");
                Console.WriteLine("Author: {0}", commit.Author.Name);
                Console.WriteLine("Message: {0}", commit.MessageShort);
            }
        }

        private static Commit GetTipOfHead()
        {
            Commit commit = null;

            using (var repo = new Repository(@"C:\Projects\Tutorials\TotalHR"))
            {
                commit = repo.Head.Tip;
                Console.WriteLine("Author: {0}", commit.Author.Name);
                Console.WriteLine("Message: {0}", commit.MessageShort);
            }

            return commit;
        }

        private static void ListContentOfCommit()
        {

            using (var repo = new Repository(@"C:\Projects\Tutorials\TotalHR"))
            {
                var commit = repo.Head.Tip;

                foreach (TreeEntry treeEntry in commit.Tree)
                {
                    Console.WriteLine("Path:{0} - Type:{1}", treeEntry.Path, treeEntry.TargetType);
                }
            }
        }

        private static void ListBranches(string path)
        {
            var repos = new Repository(path);

            var branches = repos.Branches;

            foreach (var branch in branches)
            {
                Console.WriteLine("remote: {0}", branch.FriendlyName);
            }
        }

        private static void AddToDebugBox(string text)
        {
            Console.WriteLine(text);
        }

        

    }
}



