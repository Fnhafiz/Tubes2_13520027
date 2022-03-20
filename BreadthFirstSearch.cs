using Microsoft.Msagl.Drawing;
using System.Collections.Generic;
using System.IO;
using Color = Microsoft.Msagl.Drawing.Color;

namespace Tubes2_13520027
{
    public class BFS
    {
        public static void Find(string startingPath, string target, Graph graph, List<string> answer, bool IsFindAll)
        {
            Queue<string> paths = new();
            paths.Enqueue(startingPath);

            while (paths.Count > 0)
            {
                string path = paths.Dequeue();
                string[] filesPath = Directory.GetFiles(path);
                foreach (string filePath in filesPath)
                {
                    graph.AddEdge(Path.GetFileName(path), Path.GetFileName(filePath));
                    if (Path.GetFileName(filePath) == target)
                    {
                        answer.Add(filePath);
                        graph.FindNode(Path.GetFileName(filePath)).Attr.FillColor = Color.Green;
                        if (!IsFindAll)
                        {
                            return;
                        }
                    }
                }
                string[] dirs = Directory.GetDirectories(path);
                foreach (string dir in dirs)
                {
                    paths.Enqueue(dir);
                    graph.AddEdge(Path.GetFileName(path), Path.GetFileName(dir));
                }
            }
        }
    }
}
