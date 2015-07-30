/*  Copyright (C) 2015  M. Ali & M. Shoaib

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using ScintillaNET;

namespace GUI_Experiment
{
   
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private Metrics metrics = new Metrics();
        private BackgroundWorker worker;
        private BackgroundWorker worker1;
        private System.Windows.Threading.Dispatcher disp;
        private DisplayFileDelegate update;
        private delegate void DisplayFileDelegate();
        static List<OutputData> outputList = new List<OutputData>();
        
        #endregion

        #region Constructor
    
        public MainWindow()
        {
            InitializeComponent();
        }
    
        #endregion

        #region Commands

        private void OpenFileCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            disp = openFile.Dispatcher;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = "cpp";
            dlg.Filter = "C/C++/Java (*.c,*.cpp,*.java)|*.c;*.cpp;*.java|All Files (*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                metrics.MyData.NameOfFile = dlg.FileName;

                worker.RunWorkerAsync();
            }

        }
        private void OpenFolder_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = @"C:\";
            DialogResult result = folderDialog.ShowDialog();
            if (result.ToString() == "OK")
            {

                //treev.Items.Clear();
                metrics.MyData.SelectedFolderPath = folderDialog.SelectedPath;
                var rootDirectoryInfo = new DirectoryInfo(folderDialog.SelectedPath);
                treev.Items.Add(Metrics.CreateDirectoryNode(rootDirectoryInfo));
                metrics.readFilesFromFolder();

                //System.Windows.MessageBox.Show("completed");
            }
        }
        private void ProcessFolder_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            metrics.MyData.NumOfLinesOfAllFiles = 0;
            metrics.MyData.TotalWordsInFile = 0;
            disp = openFile.Dispatcher;
            worker1 = new BackgroundWorker();
            worker1.WorkerReportsProgress = true;
            worker1.WorkerSupportsCancellation = true;
            worker1.DoWork += new DoWorkEventHandler(worker1_DoWork);
            worker1.ProgressChanged += new ProgressChangedEventHandler(worker1_ProgressChanged);
            worker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker1_RunWorkerCompleted);

            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            folderDialog.SelectedPath = @"C:\";
            DialogResult result = folderDialog.ShowDialog();
            if (result.ToString() == "OK")
            {
                metrics.MyData.SelectedFolderPath = folderDialog.SelectedPath;
                var rootDirectoryInfo = new DirectoryInfo(folderDialog.SelectedPath);
                treev.Items.Add(Metrics.CreateDirectoryNode(rootDirectoryInfo));
                metrics.readFilesFromFolder();

                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = "Document Name"; // Default file name
                dlg.DefaultExt = ".csv"; // Default file extension
                dlg.Filter = "CSV (.csv)|*.csv"; // Filter files by extension

                // Show save file dialog box
                Nullable<bool> resultOf = dlg.ShowDialog();

                // Process save file dialog box results
                if (resultOf == true)
                {
                    worker1.RunWorkerAsync(dlg.FileName);
                }

            }
        }
        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        private void AboutCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AboutWindow ab = new AboutWindow();
            ab.ShowDialog();
           // AboutBox1 ab = new AboutBox1();
            //ab.ShowDialog();
        }

        #endregion

        #region Event Handlers

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            scint.ConfigurationManager.Language = "c++";
            scint.ConfigurationManager.Configure();
            
            scint1.ConfigurationManager.Language = "c++";
            scint1.ConfigurationManager.Configure();
            
        }

        private void treev_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedTVI = (TreeViewItem)treev.SelectedItem;

            System.Windows.Controls.TreeView tv = (System.Windows.Controls.TreeView)sender;
            string text = string.Empty;
            TreeViewItem child = (TreeViewItem)tv.SelectedItem;
            if (child.Parent.GetType() == typeof(TreeViewItem)) // verify that parent is TreeViewItem
            {
                TreeViewItem parent = (TreeViewItem)child.Parent;
                text = parent.Header.ToString();
            }

            string visit = string.Empty;
            string head = selectedTVI.Header.ToString();
            if (selectedTVI.Header.ToString().Contains(".cpp") || selectedTVI.Header.ToString().Contains(".CPP")
                || selectedTVI.Header.ToString().Contains(".c") || selectedTVI.Header.ToString().Contains(".C")
                || selectedTVI.Header.ToString().Contains(".h") || selectedTVI.Header.ToString().Contains(".H")
                || selectedTVI.Header.ToString().Contains(".hpp") || selectedTVI.Header.ToString().Contains(".HPP")
                || selectedTVI.Header.ToString().Contains(".java") || selectedTVI.Header.ToString().Contains(".JAVA"))
            {
                foreach (string s in metrics.MyData.AllFileNames)
                {
                    if (s.Contains(selectedTVI.Header.ToString()))
                    {
                        visit = s;
                    }

                }
                metrics.MyData.NameOfFile = visit;
                metrics.readSingleFile();
                metrics.doCalculationsAndDisplay(visit);
                displayStats();

            }
        }

        #endregion

        #region BackgroundWorker Event Handlers

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            metrics.readSingleFile();
            metrics.doCalculationsAndDisplay(metrics.MyData.NameOfFile);
            update = new DisplayFileDelegate(DisplayFile);
            disp.BeginInvoke(update, System.Windows.Threading.DispatcherPriority.Normal);

        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ploc.Text = metrics.MyData.FileStatistics[0];
            ch.Text = metrics.MyData.FileStatistics[1];
            statemen.Text = metrics.MyData.FileStatistics[2];
            iteratI.Text = metrics.MyData.FileStatistics[3];
            Tabs.Text = metrics.MyData.FileStatistics[4];
            Spaces.Text = metrics.MyData.FileStatistics[5];
            avgll.Text = metrics.MyData.FileStatistics[6];
            cmnts.Text = metrics.MyData.FileStatistics[7];
            cmntlocl.Text = metrics.MyData.FileStatistics[8];
            loc.Text = metrics.MyData.FileStatistics[9];
            whiteRatio.Text = metrics.MyData.FileStatistics[10];
            ElNel.Text = metrics.MyData.FileStatistics[11];
            tabsTochars.Text = metrics.MyData.FileStatistics[12];
            spacesTochars.Text = metrics.MyData.FileStatistics[13];
            ElToChars.Text = metrics.MyData.FileStatistics[14];
            uppercasePercnt.Text = metrics.MyData.FileStatistics[15];
            words1.Text = metrics.MyData.FileStatistics[16];
            AvgwordsPerLine.Text = metrics.MyData.FileStatistics[17];
            charsInWord.Text = metrics.MyData.FileStatistics[18];
           
        }

        #endregion

        #region BackgroundWorker1 Event Handlers

        void worker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string path = e.Argument as string;
            var csv = new StringBuilder();
            var newLine = string.Format("File,SLOC-P,SLOC-L,Chars,Statmnts,i_Iterator,Tabs,Spaces,AvgLineLength(C),AvgLineLen(W),Comments,Words,W/NW,(EL/Non-EL),Comments/Sloc-L,(Tabs/Chars),(Spaces/Chars),(EL/Chars),AvgWordLen(chars),Uppercase%");
            csv.AppendLine(newLine);
            outputList.Clear();
            //in your loop
            int i = 0;
            foreach (string file in metrics.MyData.AllFileNames)
            {
                i++;
                metrics.MyData.AllTextLines = File.ReadAllLines(file);
                metrics.MyData.NumOfLines = metrics.MyData.AllTextLines.Length;
                metrics.MyData.FileStatistics[0] = metrics.MyData.NumOfLines.ToString();
                metrics.MyData.NumOfLinesOfAllFiles = metrics.MyData.NumOfLinesOfAllFiles + (ulong)metrics.MyData.AllTextLines.Length;

                metrics.MyData.FileText = File.ReadAllText(file);
                metrics.statistics();
                var newLine1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}",
                    Path.GetFileName(file),
                    metrics.MyData.FileStatistics[0],
                    metrics.MyData.FileStatistics[9],
                    metrics.MyData.FileStatistics[1],
                    metrics.MyData.FileStatistics[2],
                    metrics.MyData.FileStatistics[3],
                    metrics.MyData.FileStatistics[4],
                    metrics.MyData.FileStatistics[5],
                    metrics.MyData.FileStatistics[6],
                    metrics.MyData.FileStatistics[17],
                    metrics.MyData.FileStatistics[7],
                    metrics.MyData.FileStatistics[16],
                    metrics.MyData.FileStatistics[10],
                    metrics.MyData.FileStatistics[11],
                    metrics.MyData.FileStatistics[8],
                    metrics.MyData.FileStatistics[12],
                    metrics.MyData.FileStatistics[13],
                    metrics.MyData.FileStatistics[14],
                    metrics.MyData.FileStatistics[18],
                    metrics.MyData.FileStatistics[15]);

                csv.AppendLine(newLine1);
                
                outputList.Add(new OutputData() 
                {   FileName = Path.GetFileName(file),
                    Sloc_P = metrics.MyData.FileStatistics[0],
                    Sloc_L = metrics.MyData.FileStatistics[9],
                    Chars = metrics.MyData.FileStatistics[1],
                    Statements = metrics.MyData.FileStatistics[2],
                    i_Iterator = metrics.MyData.FileStatistics[3],
                    Tabs = metrics.MyData.FileStatistics[4],
                    Spaces = metrics.MyData.FileStatistics[5],
                    AvgLineLength_C = metrics.MyData.FileStatistics[6],
                    AvgLineLen_W = metrics.MyData.FileStatistics[17],
                    Comments = metrics.MyData.FileStatistics[7],
                    Words = metrics.MyData.FileStatistics[16],
                    W_to_NW = metrics.MyData.FileStatistics[10],
                    El_to_NonEl = metrics.MyData.FileStatistics[11],
                    CommentsToSloc_L = metrics.MyData.FileStatistics[8],
                    Tabs_to_Chars = metrics.MyData.FileStatistics[12],
                    Spaces_to_Chars = metrics.MyData.FileStatistics[13],
                    El_to_Chars = metrics.MyData.FileStatistics[14],
                    AvgWordLen_C = metrics.MyData.FileStatistics[18],
                    UppercasePercent = metrics.MyData.FileStatistics[15]
                });
                (sender as BackgroundWorker).ReportProgress(i);
            }

            //after your loop
            File.WriteAllText(path, csv.ToString());

        }

        void worker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            double dProgress = ((double)e.ProgressPercentage / metrics.MyData.AllFileNames.Length) * 100;
            mainProgressBar.Value = dProgress;
        }

        void worker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("Stats Writing Completed !");
            System.Windows.Controls.DataGrid mydataGrid1 = new System.Windows.Controls.DataGrid();
            mydataGrid1.AutoGenerateColumns = true;
            mydataGrid1.CanUserAddRows = false;
            mydataGrid1.ItemsSource = outputList;
            dataGridGrid.Children.Add(mydataGrid1);
            mainProgressBar.Value = 0;
        }


        #endregion

        #region Delegates

        public void DisplayFile()
        {
            scint.ResetText();
            scint.Text = metrics.MyData.FileText;
            scint.ConfigurationManager.Language = "cpp";
            scint.ConfigurationManager.Configure();
            scint.Margins[0].Width = 50;

            scint1.ResetText();
            scint1.Text = metrics.B.ToString();
            scint1.ConfigurationManager.Language = "cpp";
            scint1.ConfigurationManager.Configure();
            scint1.Margins[0].Width = 50;

        }

        #endregion

        private void displayStats()
        {
            ploc.Text = metrics.MyData.FileStatistics[0];
            ch.Text = metrics.MyData.FileStatistics[1];
            statemen.Text = metrics.MyData.FileStatistics[2];
            iteratI.Text = metrics.MyData.FileStatistics[3];
            Tabs.Text = metrics.MyData.FileStatistics[4];
            Spaces.Text = metrics.MyData.FileStatistics[5];
            avgll.Text = metrics.MyData.FileStatistics[6];
            cmnts.Text = metrics.MyData.FileStatistics[7];
            cmntlocl.Text = metrics.MyData.FileStatistics[8];
            loc.Text = metrics.MyData.FileStatistics[9];
            whiteRatio.Text = metrics.MyData.FileStatistics[10];
            ElNel.Text = metrics.MyData.FileStatistics[11];
            tabsTochars.Text = metrics.MyData.FileStatistics[12];
            spacesTochars.Text = metrics.MyData.FileStatistics[13];
            ElToChars.Text = metrics.MyData.FileStatistics[14];
            uppercasePercnt.Text = metrics.MyData.FileStatistics[15];
            words1.Text = metrics.MyData.FileStatistics[16];
            AvgwordsPerLine.Text = metrics.MyData.FileStatistics[17];
            charsInWord.Text = metrics.MyData.FileStatistics[18];

            scint.ResetText();
            scint.Text = metrics.MyData.FileText;
            scint.ConfigurationManager.Language = "cpp";
            scint.ConfigurationManager.Configure();
            scint.Margins[0].Width = 50;
            
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window1 = new Window();

            System.Windows.Controls.DataGrid mydataGrid = new System.Windows.Controls.DataGrid();
            mydataGrid.AutoGenerateColumns = true;
            mydataGrid.CanUserAddRows = false;
            mydataGrid.ItemsSource = outputList;
            Grid DynamicGrid = new Grid();
            DynamicGrid.Children.Add(mydataGrid);
            window1.Content = DynamicGrid;
            window1.Show();
        }

    }
}
