using System;
using System.IO;
using RedditSharp;
using RedditSharp.Things;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ReddFS;

public class RedditFsRetrieve
{
    Subreddit sub;
    String dirpath;
    Reddit reddit;

    public RedditFsRetrieve(Reddit r, Subreddit s, String d)
    {
        reddit = r;
        sub = s;
        dirpath = d;
    }


    public void retrieveFile(String filename)
    {
        Post post = RedditFSUtils.lookupFile(filename, sub);
        if (post == null)
        {
            Console.WriteLine("Failed to find file specified.");
        }
        else
        {
            var output = File.Create(dirpath + filename);
            Comment currComment = post.Comments.First();
            String prevId = currComment.Id;
            int i = 0;
            do
            {
                Console.Write("Reading block " + i + "...");
                byte[] b;
                try
                {
                    b = Convert.FromBase64String(currComment.Body);
                }
                catch(ArgumentNullException)
                {
                    //So this is a rather meaty catch block. Essentially, redditSharp only fetches comment chains 10 deep as far as I can tell.
                    //This catch block essentially just re-retrieves the comment chain where it was left off when that occurs.
                    Console.WriteLine("Comment with parent id " + prevId + " was null. Attemping to refetch from post " + post.Id);
                    var thing = (Comment)reddit.GetThingByFullname(currComment.ParentId);
                    var parentFull = reddit.GetComment(new System.Uri(thing.Shortlink));
                    
                    if (parentFull != null)
                    {
                        Console.WriteLine("Successfully retrieved new root. Retrying block.");
                        try
                        {
                            Console.Write("Reading block " + i + "...");
                            currComment = parentFull.Comments.First();
                            b = Convert.FromBase64String(currComment.Body);
                        }
                        catch(ArgumentNullException)
                        {
                            Console.WriteLine("Failed again after failover. Aborting.");
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed to find parent in root. Aborting.");
                        return;
                    }

                }
                Console.WriteLine(" writing buffer of size " + b.Length + "B to disk");
                output.Write(b, 0, b.Length);
                i++; //Block counter;
                prevId = currComment.Id; //need this for recovery
                currComment = currComment.Comments.Count > 0 ? currComment.Comments.First() :  null;
            } while (currComment != null);

            Console.Write("Retrieved file " + filename + " to dir " + dirpath);
        }
    }
}
