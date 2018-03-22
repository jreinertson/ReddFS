using RedditSharp;
using RedditSharp.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReddFS
{
    class Shell
    {
        Reddit reddit;
        Subreddit sub;
        Boolean cont = true;

        public Shell(Reddit r, Subreddit s)
        {
            reddit = r;
            sub = s;
        }

        public void startShell()
        {
            Subreddit dir = sub;
            while (cont)
            {
                Console.Write(dir.Name + ">");
                var s = Console.ReadLine();
                string[] args = s.Split(' ');
                handleInput(args, dir);
            }
        }


        private void handleInput(string[] args, Subreddit dir)
        {
            if (args.Length == 0)
            {
                return;
            }
            else if(args.Length == 1)
            {
                if (args[0].Equals("ls"))
                {
                    ls(dir);
                }
                else if (args[0].Equals("exit"))
                {
                    cont = false;
                }
            }
            else if (args.Length == 2)
            {
                if (args[0].Equals("cd"))
                {
                    cd(args[1], dir);
                }
            }
            else
            {
                Console.WriteLine("Invalid num args");
            }
        }

        private void ls(Subreddit dir)
        {
            var posts = sub.New.GetEnumerator(100);
            while (posts.MoveNext())
            {
                if (!RedditFSUtils.isDirectory(posts.Current))
                {
                    Console.WriteLine("\t" + posts.Current.Title);
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.WriteLine("\t" + posts.Current.Title);
                    Console.ResetColor();
                }
            }
        }
        

        private void cd(String filename, Subreddit dir)
        {
            var file = RedditFSUtils.lookupFile(filename, dir);
            if (file != null)
            {
                if (RedditFSUtils.isDirectory(file))
                {
                    var newDir = RedditFSUtils.openDirSymlink(reddit, file);
                    if (newDir != null)
                    {
                        dir = newDir;
                    }
                    else
                    {
                        Console.WriteLine("Failed to open dir.");
                    }
                }
                else
                {
                    Console.WriteLine("Operand to cd was not a dir.");
                }
            }
            else
            {
                Console.WriteLine("No such file/dir exists.");
            }
        }
    }
}
