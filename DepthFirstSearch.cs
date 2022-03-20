using Microsoft.Msagl.Drawing;
using System.Collections.Generic;
using System.IO;
using Color = Microsoft.Msagl.Drawing.Color;

namespace Tubes2_13520027
{
    public class DFS
    {
        public static void Find(string startingPath, string target, Graph graph, List<string> result, bool IsFindAll)
        {
            string[] files = Directory.GetFiles(startingPath);
            foreach (string file in files)
            {
                graph.AddEdge(Path.GetFileName(startingPath), Path.GetFileName(file));
                if (Path.GetFileName(file) == target)
                {
                    result.Add(file);
                    graph.FindNode(Path.GetFileName(file)).Attr.FillColor = Color.Green;
                    if (!IsFindAll)
                    {
                        return;
                    }
                }
            }
            string[] dirs = Directory.GetDirectories(startingPath);
            foreach (string dir in dirs)
            {
                graph.AddEdge(Path.GetFileName(startingPath), Path.GetFileName(dir));
                Find(dir, target, graph, result, IsFindAll);
                if (result.Count == 1 && !IsFindAll)
                {
                    return;
                }
            }
        }
    }
}
