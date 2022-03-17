using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tubes2_13520027
{
    public class BFS
    {
        //public static void Main(string[] args)
        //{
        //    Console.WriteLine(Search("C:\\Users\\Kent\\Documents\\GitHub", "Programs.cs"));
        //}

        public static string Search(string startingPath, string target)
        {
            Queue<string> paths = new Queue<string>();
            paths.Enqueue(startingPath);

            while (paths.Count > 0)
            {
                string path = paths.Dequeue();
                string[] filesPath = Directory.GetFiles(@path, "*");
                foreach (string filePath in filesPath)
                {
                    if (Path.GetFileName(filePath) == target)
                    {
                        return filePath;
                    }
                }
                string[] dirs = Directory.GetDirectories(@path, "*", SearchOption.TopDirectoryOnly);
                foreach (string dir in dirs)
                {
                    paths.Enqueue(dir);
                }
            }

            return "Not Found";
        }
    }
}
