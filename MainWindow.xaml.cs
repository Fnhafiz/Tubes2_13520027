using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using System.Diagnostics;
using System.Windows;
using System.IO;
using System.Windows.Forms;

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

                // Start Stopwatch
                Stopwatch watch = new();
                watch.Start();

                if (chkFind.IsChecked == true)
                {
                    if (rdrBFS.IsChecked == true)
                    {
                        method.Text = "Method: BFS Find All Occurrences";
                        // BFS(txtBoxFolder.Text, true);
                        BFS.FindAllOccurrences(txtBoxFolder.Text, txtBoxFile.Text, graph);
                    }
                    else
                    {
                        method.Text = "Method: DFS Find All Occurrences";
                        // DFS(txtBoxFolder.Text, true);
                    }
                }
                else
                {
                    if (rdrBFS.IsChecked == true)
                    {
                        method.Text = "Method: BFS";
                        // BFS(txtBoxFolder.Text, false);
                    }
                    else
                    {
                        method.Text = "Method: DFS";
                        // DFS(txtBoxFolder.Text, false);
                    }
                }

                // Stop Stopwatch
                watch.Stop();

                // Place Graph into Panel
                graphViewer.Graph = graph;

                // Add Stats
                pathfile.Text = $"Path File: ";
                path.Visibility = Visibility.Visible;
                txtBoxLink.Text = BFS.answer;
                time.Text = $"Time Spent: {watch.ElapsedMilliseconds} ms";

                progress.IsIndeterminate = false;
            }
            else
            {
                System.Windows.MessageBox.Show("Starting directory is not valid", "Crawler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", txtBoxLink.Text);
        }

    }
}
