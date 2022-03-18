using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.WpfGraphControl;
using Color = Microsoft.Msagl.Drawing.Color;
using Shape = Microsoft.Msagl.Drawing.Shape;
using Tubes2_13520027;

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
            //Loaded += MainWindow_Loaded;
        }

        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var graph = new Graph();

        //    graph.AddEdge("A", "B");
        //    graph.AddEdge("B", "C");
        //    graph.AddEdge("A", "C").Attr.Color = Color.Green;
        //    graph.FindNode("A").Attr.FillColor = Color.Magenta;
        //    graph.FindNode("B").Attr.FillColor = Color.MistyRose;
        //    graph.FindNode("A").Attr.Color = Color.Red;
        //    Node c = graph.FindNode("C");
        //    c.Attr.FillColor = Color.PaleGreen;
        //    c.Attr.Shape = Shape.Diamond;
        //    graphControl.Graph = graph;
        //}

        private void GraphDirectoryBFS(string dir, Graph graph, string target)
        {
            string [] subdirs = Directory.GetDirectories(dir);
            string answer = BFS.Search(dir, target);

            foreach (string subdir in subdirs)
            {
                graph.AddEdge(System.IO.Path.GetFileName(dir), System.IO.Path.GetFileName(subdir));
                string[] files = Directory.GetFiles(subdir);

                foreach (string file in files)
                {
                    if (answer == file)
                    {
                        graph.AddEdge(System.IO.Path.GetFileName(subdir), System.IO.Path.GetFileName(file));
                        txtBoxLink.Text = subdir;
                        //    graph.FindNode(System.IO.Path.GetFileName(subdir)).Attr.Color = Color.Green;
                        //    graph.AddEdge(System.IO.Path.GetFileName(dir), System.IO.Path.GetFileName(subdir)).Attr.Color = Color.Green;
                        //    graph.AddEdge(System.IO.Path.GetFileName(subdir), System.IO.Path.GetFileName(file)).Attr.Color = Color.Green;
                    }
                    else
                    {
                        graph.AddEdge(System.IO.Path.GetFileName(subdir), System.IO.Path.GetFileName(file));
                    }
                }
                GraphDirectoryBFS(subdir, graph, target);

            }
            if (answer != "Not Found")
            {
                graph.FindNode(System.IO.Path.GetFileName(answer)).Attr.FillColor = Color.Green;
            }


        }


        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            var graph = new Graph();
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            folderDlg.Description = "Choose Starting Directory";
            folderDlg.UseDescriptionForTitle = true;
            folderDlg.InitialDirectory = "file://";
            folderDlg.ShowDialog();

            // Change txtBoxFolder to Path
            txtBoxFolder.Text = folderDlg.SelectedPath;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var graph = new Graph();
            progress.IsIndeterminate = true;
            var watch = new Stopwatch();
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
                    graphControl.Graph = graph;
                }
                else
                {
                    method.Text = "Method: DFS";
                    // DFS(txtBoxFolder.Text, false);
                }
            }
            watch.Stop();
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
