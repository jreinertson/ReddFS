using System;
using ReddFS;
using RedditSharp;

namespace RedditFS
{
    class Program
    {
        static void Main(string[] args)
        {
            String user = System.Configuration.ConfigurationSettings.AppSettings["username"];
            String pass = System.Configuration.ConfigurationSettings.AppSettings["password"];
            String sub = System.Configuration.ConfigurationSettings.AppSettings["subreddit"];
            String secret = System.Configuration.ConfigurationSettings.AppSettings["secret"];
            String client = System.Configuration.ConfigurationSettings.AppSettings["client"];
            String redirect = System.Configuration.ConfigurationSettings.AppSettings["redirect"];
            String storeDir = System.Configuration.ConfigurationSettings.AppSettings["storageDir"];
            int maxBlockSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["maxBlockSize"]);


            var webAgent = new BotWebAgent(user, pass, client, secret, redirect);
            var reddit = new Reddit(webAgent, true);
            var subreddit = reddit.GetSubreddit(sub);

            RedditFsStore store = new RedditFsStore(subreddit, maxBlockSize);
            RedditFsRetrieve retrieve = new RedditFsRetrieve(reddit, subreddit, storeDir);
            Shell shell = new Shell(reddit, subreddit);

            if (args.Length == 2)
            {
                if (args[0] == "store")
                {
                    Console.WriteLine("Attempting to store file at root: " + args[1]);
                    store.storeFile(args[1]);
                }
                else if (args[0] == "retrieve")
                {
                    Console.WriteLine("Attempting to retrive file into current dir: " + args[1]);
                    retrieve.retrieveFile(args[1]);
                }
            }
            else if (args.Length == 1) { 
                if (args[0] == "shell")
                {
                    Console.WriteLine("h e l l o  r e d d i t");
                    shell.startShell();
                }
            }
            else
            {
                Console.WriteLine("Unsupported op.");
            }
        }
    }
}
