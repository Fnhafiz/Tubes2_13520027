using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Tubes2_13520027
{
    public class DepthFirstSearch
    {
        public List<string> DFS(string startingPath, string target)
        {
            List<string> result = new List<string>();
            string[] files = Directory.GetFiles(startingPath);
            foreach(string file in files)
            {
                if(Path.GetFileName(file) == target)
                {
                    result.Add(file);
                }
            }
            string[] dirs = Directory.GetDirectories(startingPath);
            foreach (string dir in dirs)
            {
                result.Concat(DFS(dir, target));
            }


            return result;
        }
        /*
        public List<string> Search(string startingPath, string target)
        {
            List<string> result = new List<string>();


            return 
        }
        */
    }
}
