using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;

namespace Tubes2_13520027
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AllFileDirectories(string startingPath, List<string> AllFile)
        {
            string[] subdirs = Directory.GetDirectories(startingPath);
            foreach (string subdir in subdirs)
            {
                AllFile.Add(subdir);
                string[] files = Directory.GetFiles(subdir);

                foreach (string file in files)
                {
                    AllFile.Add(file);
                }
                AllFileDirectories(subdir, AllFile);
            }
        }

        private void GraphDirectory(string startingPath, List<string> visited, List<string> answer, Graph graph)
        {
            var visitedDistinct = visited.Distinct().ToList();
            List<string> answerDistinct = new();
            List<string> allFile = new();
            AllFileDirectories(startingPath, allFile);
            bool found = false;

            // Warnain yang answer (hijau)
            for (int i = 0; i < answer.Count; i++)
            {
                bool found2 = false;
                string ans = answer[i];
                while (ans != null && ans != startingPath)
                {
                    for (int j = 0; j < answerDistinct.Count; j++)
                    {
                        if (ans == answerDistinct[j])
                        {
                            found2 = true;
                        }
                    }
                    if (!found2)
                    {
                        graph.AddEdge(Path.GetFileName(Path.GetDirectoryName(ans)), Path.GetFileName(ans)).Attr.Color = Color.Green;
                        graph.FindNode(Path.GetFileName(ans)).Attr.FillColor = Color.Green;
                        visitedDistinct.Remove(ans);
                        allFile.Remove(ans);
                        found = true;
                        answerDistinct.Add(ans);
                    }
                    ans = Path.GetDirectoryName(ans);
                }
            }

            // Warnain yang visited (merah) tapi bukan answer
            for (int i = 0; i < visitedDistinct.Count; i++)
            {
                if (visitedDistinct[i] != startingPath)
                {
                    allFile.Remove(visitedDistinct[i]);
                    graph.AddEdge(Path.GetFileName(Path.GetDirectoryName(visitedDistinct[i])), Path.GetFileName(visitedDistinct[i])).Attr.Color = Color.Red;
                    graph.FindNode(Path.GetFileName(visitedDistinct[i])).Attr.FillColor = Color.Red;
                }
            }

            // Warnain sisanya
            for (int i = 0; i < allFile.Count; i++)
            {
                if (allFile[i] != startingPath)
                {
                    graph.AddEdge(Path.GetFileName(Path.GetDirectoryName(allFile[i])), Path.GetFileName(allFile[i]));
                }
            }

            // Warnain Starting Path -> Hijau atau Merah
            if (found)
            {
                graph.FindNode(Path.GetFileName(startingPath)).Attr.FillColor = Color.Green;
            }
            else
            {
                graph.FindNode(Path.GetFileName(startingPath)).Attr.FillColor = Color.Red;
            }

        }

        private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {
            // Open Folder Selector Dialog
            FolderBrowserDialog folderDlg = new();
            folderDlg.ShowNewFolderButton = true;
            folderDlg.Description = "Choose Starting Directory";
            folderDlg.UseDescriptionForTitle = true;
            if (Directory.Exists(txtBoxFolder.Text))
            {
                folderDlg.InitialDirectory = txtBoxFolder.Text;
            }
            else
            {
                folderDlg.InitialDirectory = "file://";
            }
            folderDlg.ShowDialog();

            // Change txtBoxFolder to Path
            txtBoxFolder.Text = folderDlg.SelectedPath;
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(txtBoxFolder.Text))
            {
                progress.IsIndeterminate = true;

                // Create Graph Panel
                GraphViewer graphViewer = new();
                graphViewer.BindToPanel(graphViewerPanel);

                // Create Graph
                Graph graph = new();

                // Create Answer
                List<string> answer = new();

                // Create Visited
                List<string> visited = new();

                // Start Stopwatch
                Stopwatch watch = new();
                watch.Start();

                if (chkFind.IsChecked == true)
                {
                    if (rdrBFS.IsChecked == true)
                    {
                        method.Text = "Method: BFS Find All Occurrences";
                        BFS.Find(txtBoxFolder.Text, txtBoxFile.Text, visited, answer, true);
                        GraphDirectory(txtBoxFolder.Text, visited, answer, graph);
                    }
                    else // rdrDFS.IsChecked == true
                    {
                        method.Text = "Method: DFS Find All Occurrences";
                        DFS.Find(txtBoxFolder.Text, txtBoxFile.Text, visited, answer, true);
                        GraphDirectory(txtBoxFolder.Text, visited, answer, graph);
                    }
                }
                else // chkFind.IsChecked == false
                {
                    if (rdrBFS.IsChecked == true)
                    {
                        method.Text = "Method: BFS";
                        BFS.Find(txtBoxFolder.Text, txtBoxFile.Text, visited, answer, false);
                        GraphDirectory(txtBoxFolder.Text, visited, answer, graph);
                    }
                    else // rdrDFS.IsChecked == true
                    {
                        method.Text = "Method: DFS";
                        DFS.Find(txtBoxFolder.Text, txtBoxFile.Text, visited, answer, false);
                        GraphDirectory(txtBoxFolder.Text, visited, answer, graph);
                    }
                }

                // Stop Stopwatch
                watch.Stop();

                // Place Graph into Panel
                graphViewer.Graph = graph;

                // Add Stats
                pathfile.Text = $"Path File: ";
                listAnswer.Visibility = Visibility.Visible;
                listAnswer.ItemsSource = answer;
                time.Text = $"Time Spent: {watch.ElapsedMilliseconds} ms";

                progress.IsIndeterminate = false;
            }
            else
            {
                System.Windows.MessageBox.Show("Starting directory is not valid", "Crawler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Path.GetDirectoryName(e.Uri.AbsoluteUri),
                UseShellExecute = true,
            });
            e.Handled = true;
        }

    }
}
