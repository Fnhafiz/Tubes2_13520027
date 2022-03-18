using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Color = Microsoft.Msagl.Drawing.Color;

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

        private void GraphDirectoryBFS(string dir, Graph graph, string target)
        {
            string[] subdirs = Directory.GetDirectories(dir);
            string answer = BFS.Search(dir, target);

            foreach (string subdir in subdirs)
            {
                graph.AddEdge(Path.GetFileName(dir), Path.GetFileName(subdir));
                string[] files = Directory.GetFiles(subdir);

                foreach (string file in files)
                {
                    if (answer == file)
                    {
                        graph.AddEdge(Path.GetFileName(subdir), Path.GetFileName(file));
                        txtBoxLink.Text = subdir;
                        //    graph.FindNode(Path.GetFileName(subdir)).Attr.Color = Color.Green;
                        //    graph.AddEdge(Path.GetFileName(dir), Path.GetFileName(subdir)).Attr.Color = Color.Green;
                        //    graph.AddEdge(Path.GetFileName(subdir), Path.GetFileName(file)).Attr.Color = Color.Green;
                    }
                    else
                    {
                        graph.AddEdge(Path.GetFileName(subdir), Path.GetFileName(file));
                    }
                }
                GraphDirectoryBFS(subdir, graph, target);
            }
            if (answer != "Not Found")
            {
                graph.FindNode(Path.GetFileName(answer)).Attr.FillColor = Color.Green;
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
            progress.IsIndeterminate = true;
            GraphViewer graphViewer = new();
            graphViewer.BindToPanel(graphViewerPanel);
            Graph graph = new();
            Stopwatch watch = new();
            watch.Start();
            // BFS/DFS(<folder path>, isChecked);
            if (chkFind.IsChecked == true)
            {
                if (rdrBFS.IsChecked == true)
                {
                    method.Text = "Method: BFS Find All Occurence";
                    // BFS(txtBoxFolder.Text, true);
                }
                else
                {
                    method.Text = "Method: DFS Find All Occurence";
                    // DFS(txtBoxFolder.Text, true);
                }
            }
            else
            {
                if (rdrBFS.IsChecked == true)
                {
                    method.Text = "Method: BFS";
                    // BFS(txtBoxFolder.Text, false);
                    GraphDirectoryBFS(txtBoxFolder.Text, graph, txtBoxFile.Text);
                }
                else
                {
                    method.Text = "Method: DFS";
                    // DFS(txtBoxFolder.Text, false);
                }
            }
            watch.Stop();
            graphViewer.Graph = graph;
            pathfile.Text = $"Path File: ";
            path.Visibility = Visibility.Visible;
            time.Text = $"Time Spent: {watch.ElapsedMilliseconds} ms";
            progress.IsIndeterminate = false;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", txtBoxLink.Text);
        }

    }
}
