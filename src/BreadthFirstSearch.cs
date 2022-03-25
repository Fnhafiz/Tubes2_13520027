using System.Collections.Generic;
using System.IO;

namespace Tubes2_13520027
{
    public class BFS
    {
        public static void Find(string startingPath, string target, List<string> visited, List<string> answer, bool IsFindAll)
        {
            Queue<string> paths = new();
            paths.Enqueue(startingPath);

            while (paths.Count > 0)
            {
                string path = paths.Dequeue();
                visited.Add(path);
                string[] filesPath = Directory.GetFiles(path);
                foreach (string filePath in filesPath)
                {
                    visited.Add(filePath);
                    //graph.AddEdge(Path.GetFileName(path), Path.GetFileName(filePath));
                    if (Path.GetFileName(filePath) == target)
                    {
                        answer.Add(filePath);
                        //graph.FindNode(Path.GetFileName(filePath)).Attr.FillColor = Color.Green;
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
                    //graph.AddEdge(Path.GetFileName(path), Path.GetFileName(dir));
                }
            }
        }
    }
}
