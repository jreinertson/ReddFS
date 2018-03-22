using RedditSharp;
using RedditSharp.Things;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReddFS
{
    class RedditFSUtils
    {
        public static Post lookupFile(String filename, Subreddit dir)
        {
            var pages = dir.New.GetEnumerator(100);
            while (pages.MoveNext())
            {
                if (pages.Current.Title.Equals(filename))
                {
                    return pages.Current;
                }
            }
            Console.WriteLine("file not found.");
            return null;
        }

        public static Boolean isDirectory(Post p)
        {
            //effectively any link to something else on reddit is a symlink, either to a file or a directory
            //Although, directories can ONLY be linked by symlinks, since we can't really nest subreddits
            //Todo: more sophisticated checks, if it's actually a subreddit link, or if it's a symlink to a file
            //if domain is reddit, then:
                //if it contains /r/, then:
                    //if it contains "/comments/", then it's a symlink to a file, not a directory
                    //if it does not, then we'll consider it a directory
            return !p.IsSelfPost;
        }

        public static Boolean isFileSymlink(Post p)
        {
            //check if a post is a symbolic link to a file, eg, it should be reddit.com/r/subreddit/comments/id
            return false;
        }

        public static Subreddit openDirSymlink(Reddit reddit, Post p)
        {
            //attempt to retrieve subreddit that a symlink maps to
            return null;
        }

        public static Post openFileSymlink(Reddit reddit, Post p)
        {
            //attempt to retrieve file that a symlink maps to
            return null;
        }
    }
}
