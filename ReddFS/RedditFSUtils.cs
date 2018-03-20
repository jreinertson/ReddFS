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
            //Todo: check if url matches a subreddit
            //use domain perhaps
            return !p.IsSelfPost;
        }
    }
}
