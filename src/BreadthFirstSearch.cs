using System.Collections.Generic;
using System.IO;

namespace Tubes2_13520027
{
    public class BFS
    {
        private static void BFSDirectories(string startingPath, Queue<string> totalpaths, Queue<string> pathTemp)
        {
            List<string> dirTemp = new();
            List<string> fileTemp = new();


            string[] subdirs = Directory.GetDirectories(startingPath);
            foreach (string subdir in subdirs)
            {
                dirTemp.Add(subdir);
            }

            string[] files = Directory.GetFiles(startingPath);
            foreach (string file in files)
            {
                fileTemp.Add(file);
            }

            while (dirTemp.Count > 0 || fileTemp.Count > 0)
            {
                if (dirTemp.Count > 0 && fileTemp.Count > 0)
                {
                    string dirTempName = dirTemp[0];
                    string fileTempName = fileTemp[0];
                    int comp = string.Compare(dirTempName, fileTempName);
                    if (comp == -1)
                    {
                        pathTemp.Enqueue(dirTempName);
                        totalpaths.Enqueue(dirTempName);
                        dirTemp.RemoveAt(0);
                    }
                    else
                    {
                        totalpaths.Enqueue(fileTempName);
                        fileTemp.RemoveAt(0);
                    }
                }
                else if (dirTemp.Count > 0 && fileTemp.Count == 0)
                {
                    totalpaths.Enqueue(dirTemp[0]);
                    pathTemp.Enqueue(dirTemp[0]);
                    dirTemp.RemoveAt(0);
                }
                else if (dirTemp.Count == 0 && fileTemp.Count > 0)
                {
                    totalpaths.Enqueue(fileTemp[0]);
                    fileTemp.RemoveAt(0);
                }
            }
        }
        public static void Find(string startingPath, string target, List<string> visited, List<string> answer, bool IsFindAll)
        {
            Queue<string> paths = new();
            Queue<string> pathTemp = new();

            BFSDirectories(startingPath, paths, pathTemp);

            while (pathTemp.Count > 0)
            {
                string loopDir = pathTemp.Dequeue();
                BFSDirectories(loopDir, paths, pathTemp);
            }

            while (paths.Count > 0)
            {
                string pathagain = paths.Dequeue();
                visited.Add(pathagain);
                if (Path.GetFileName(pathagain) == target)
                {
                    answer.Add(pathagain);
                    if (!IsFindAll)
                    {
                        return;
                    }
                }
            }
        }
    }
}
