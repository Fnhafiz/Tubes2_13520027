using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
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

                // Create Answer
                List<string> answer = new();

                // Start Stopwatch
                Stopwatch watch = new();
                watch.Start();

                if (chkFind.IsChecked == true)
                {
                    if (rdrBFS.IsChecked == true)
                    {
                        method.Text = "Method: BFS Find All Occurrences";
                        BFS.Find(txtBoxFolder.Text, txtBoxFile.Text, graph, answer, true);
                    }
                    else // rdrDFS.IsChecked == true
                    {
                        method.Text = "Method: DFS Find All Occurrences";
                        DFS.Find(txtBoxFolder.Text, txtBoxFile.Text, graph, answer, true);
                    }
                }
                else // chkFind.IsChecked == false
                {
                    if (rdrBFS.IsChecked == true)
                    {
                        method.Text = "Method: BFS";
                        BFS.Find(txtBoxFolder.Text, txtBoxFile.Text, graph, answer, false);
                    }
                    else // rdrDFS.IsChecked == true
                    {
                        method.Text = "Method: DFS";
                        DFS.Find(txtBoxFolder.Text, txtBoxFile.Text, graph, answer, false);
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            //Process.Start("explorer.exe", txtBoxLink.Text);
            Process.Start("explorer.exe");
        }

    }
}
