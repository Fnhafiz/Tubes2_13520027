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

        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.ShowNewFolderButton = true;
            // folderDlg.InitialDirectory = 
            // Show the FolderBrowserDialog.  
            DialogResult result = folderDlg.ShowDialog();
            // Change txtBoxFolder to Path
            txtBoxFolder.Text = folderDlg.SelectedPath;
            Environment.SpecialFolder root = folderDlg.RootFolder;

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            progress.IsIndeterminate = true;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", @"c:\users");
        }

    }
}
