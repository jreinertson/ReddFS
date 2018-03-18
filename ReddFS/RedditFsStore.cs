using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using RedditSharp;
using RedditSharp.Things;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace RedditFS
{
    class RedditFsStore
    {
        Subreddit sub;
        int maxBlockSize;

        public RedditFsStore(Subreddit s, int m)
        {
            sub = s;
            maxBlockSize = m;
        }

        public void storeFile(String filepath)
        {
            String filename = Path.GetFileName(filepath);
            String fileContents = Convert.ToBase64String(File.ReadAllBytes(filepath));
            Console.WriteLine("creating file head...");
            Post fileHead = sub.SubmitTextPost(filename, "file created by redditfs");
            Comment currComment = null;
            for (int i  = 0; i* maxBlockSize < fileContents.Length; i++)
            {
                var start = i * maxBlockSize;
                var readLen = maxBlockSize;
                if (fileContents.Length - start < readLen)
                {
                    readLen = fileContents.Length - start;
                }
                if (currComment == null)
                {
                    Console.WriteLine("creating root file block " + i + "...");
                    currComment = fileHead.Comment(fileContents.Substring(start, readLen));
                }
                else
                {
                    Console.WriteLine("creating tail file block " + i + "...");
                    currComment = currComment.Reply(fileContents.Substring(start, readLen));
                }
            }
        }
    }
}
