using Microsoft.Msagl.Drawing;
using System.Collections.Generic;
using System.IO;
using Color = Microsoft.Msagl.Drawing.Color;

namespace Tubes2_13520027
{
    public class DFS
    {
        public static void Find(string startingPath, string target, List<string> visited, List<string> result, bool IsFindAll)
        {
            visited.Add(startingPath);
            string[] files = Directory.GetFiles(startingPath);
            int i = 0;
            foreach (string file in files)
            {
                visited.Add(files[i]);
                visited.Add(file);
                //graph.AddEdge(Path.GetFileName(startingPath), Path.GetFileName(file));
                if (Path.GetFileName(file) == target)
                {
                    result.Add(file);
                    //graph.FindNode(Path.GetFileName(file)).Attr.FillColor = Color.Green;
                    if (!IsFindAll)
                    {
                        return;
                    }
                }
                i++;
            }
            string[] dirs = Directory.GetDirectories(startingPath);
            int j = 0;
            foreach (string dir in dirs)
            {
                visited.Add(dirs[j]);
                visited.Add(dir);
                //graph.AddEdge(Path.GetFileName(startingPath), Path.GetFileName(dir));
                Find(dir, target, visited, result, IsFindAll);
                if (result.Count == 1 && !IsFindAll)
                {
                    return;
                }
                j++;
            }
        }
    }
}
