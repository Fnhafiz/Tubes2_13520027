using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private void GraphInitial(string startingPath, Graph graph)
        {
            string[] subdirs = Directory.GetDirectories(startingPath);
            foreach (string subdir in subdirs)
            {
                graph.AddEdge(Path.GetFileName(startingPath), Path.GetFileName(subdir));
                string[] files = Directory.GetFiles(subdir);

                foreach (string file in files)
                {
                    graph.AddEdge(Path.GetFileName(subdir), Path.GetFileName(file));
                }
                GraphInitial(subdir, graph);
            }
        }

        private void GraphDirectory(string startingPath, List<string> visited, List<string> answer, Graph graph)
        {
            GraphInitial(startingPath, graph);

            for (int i = 0; i < visited.Count; i++)
            {
                if (graph.FindNode(Path.GetFileName(visited[i])) != null)
                {

                    if (Path.GetFileName(visited[i]) != Path.GetFileName(startingPath))
                    {
                        //string filePath = Path.GetDirectoryName(visited[i]);
                        //graph.RemoveNode(graph.FindNode(Path.GetFileName(filePath)));
                        //graph.RemoveEdge(Edge (Path.GetFileName(filePath), Path.GetFileName(visited[i])));
                        //graph.AddEdge(Path.GetFileName(filePath), Path.GetFileName(visited[i])).Attr.Color = Color.Red;
                    }
                    graph.FindNode(Path.GetFileName(visited[i])).Attr.FillColor = Color.Red;
                }
            }
            for (int i = 0; i < answer.Count; i++)
            {
                string ans = answer[i];
                while (ans != null)
                {
                    if (graph.FindNode(Path.GetFileName(ans)) != null)
                    {                    
                        graph.FindNode(Path.GetFileName(ans)).Attr.FillColor = Color.Green;
                    }
                    ans = Path.GetDirectoryName(ans);
                }
            }
        }

        private void BtnFolder_Click(object sender, RoutedEventArgs e)
        {
            // Open Folder Selector Dialog
            FolderBrowserDialog folderDlg = new();
            folderDlg.ShowNewFolderButton = true;
            folderDlg.Description = "Choose Starting Directory";
            folderDlg.UseDescriptionForTitle = true;
            folderDlg.InitialDirectory = "file://";
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
