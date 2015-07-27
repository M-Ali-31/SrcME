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
using System.IO;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using ScintillaNET;
using Microsoft.Win32;

namespace GUI_Experiment
{
    class Metrics
    {
        #region Fields
        private Data myData = new Data();
        private StringBuilder b = new StringBuilder();
        #endregion

        #region Properties
        public Data MyData
        {
            get
            {
                return myData;
            }
            set
            {
                myData = value;
            }
        }
        public StringBuilder B
        {
            get
            {
                return b;
            }
            set
            {
                b = value;
            }
        }
        #endregion

        #region Constructor
        public Metrics()
        {
           
        }
        #endregion

        #region Methods
        public void statistics()
        {
            totalChars();
            totalStatements();
            iterator_i();
            tabs();
            spaces();
            avg_line_length();
            comments();
            logicalLoc();
            ratioWNW();
            ELNEL();
            commentsToLocp();
            tabsToChars();
            spacesToChars();
            ElToCharss();
            uppercaseLettersPercentage();
            wordcounting();
        }
        private void totalChars()
        {
            int charCount = myData.FileText.Length;
            myData.FileStatistics[1] = charCount.ToString();
        }
        private void totalStatements()
        {
            int statementCount = 0;
            for (int index = 0; index < myData.FileText.Length; index++)
            {
                if (myData.FileText[index] == ';')
                    statementCount++;
            }
            myData.FileStatistics[2] = statementCount.ToString();
        }
        private void iterator_i()
        {
            int iteratorCount = 0;
            myData.NonEmptyLines = myData.FileText.Split(myData.LineTerminator, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in myData.NonEmptyLines)
            {
                if (s.Contains("i++") || s.Contains("++i") || s.Contains("[i]"))
                    iteratorCount++;
            }
            myData.FileStatistics[3] = iteratorCount.ToString();
        }
        private void tabs()
        {
            int length = myData.NonEmptyLines.Length;
            int totalTabs = 0;
            int[] numOfTabsInEveryNonEmptyLine = new int[length];
            for (int index = 0; index < length; index++)
            {
                numOfTabsInEveryNonEmptyLine[index] = 0;
            }
            int ind = 0;

            foreach (string s in myData.NonEmptyLines)
            {
                foreach (char a in s)
                {
                    if (ind < length && a == '\t')
                        numOfTabsInEveryNonEmptyLine[ind]++;
                }
                ind++;
            }
            for (int count = 0; count < numOfTabsInEveryNonEmptyLine.Length; count++)
            {
                totalTabs = totalTabs + numOfTabsInEveryNonEmptyLine[count];
            }
            myData.NumOfTabsChars = totalTabs;
            myData.FileStatistics[4] = totalTabs.ToString();
        }
        private void spaces()
        {
            int length = myData.NonEmptyLines.Length;
            int spaceCount = 0;
            int[] numOfSpacesInEveryNonEmptyLine = new int[length];
            for (int index = 0; index < length; index++)
            {
                numOfSpacesInEveryNonEmptyLine[index] = 0;
            }
            int ind = 0;

            foreach (string s in myData.NonEmptyLines)
            {
                foreach (char a in s)
                {
                    if (ind < length && a == ' ')
                        numOfSpacesInEveryNonEmptyLine[ind]++;
                }
                ind++;
            }
            for (int count = 0; count < numOfSpacesInEveryNonEmptyLine.Length; count++)
            {
                spaceCount = spaceCount + numOfSpacesInEveryNonEmptyLine[count];
            }
            myData.NumOfSpaceChars = spaceCount;
            myData.FileStatistics[5] = spaceCount.ToString();
        }
        private void avg_line_length()
        {
            int total_length = 0, total_lines = myData.NonEmptyLines.Length, avg = 0;
            foreach (string s in myData.NonEmptyLines)
            {
                total_length = total_length + s.Length;
            }
            avg = total_length / total_lines;
            myData.FileStatistics[6] = avg.ToString("0.000");
        }
        private int slashComments()
        {
            int LinesStartWithSlashCount = 0;
            int totalSlashComments = 0;
            foreach (string s in myData.NonEmptyLines)
            {
                if (s.Trim().StartsWith("//"))
                    LinesStartWithSlashCount++;
                if (s.Contains("//"))
                    totalSlashComments++;
            }
            return totalSlashComments;
        }
        private int slashStarComments()
        {
            int LinesStartWithSlashStarCount = 0;
            int totalSlashStarComments = 0;
            foreach (string s in myData.NonEmptyLines)
            {
                if (s.Trim().StartsWith("/*"))
                    LinesStartWithSlashStarCount++;
                if (s.Contains("/*"))
                    totalSlashStarComments++;
            }
            return totalSlashStarComments;
        }
        private void comments()
        {
            int totalComments = slashComments() + slashStarComments();
            myData.FileStatistics[7] = totalComments.ToString();
        }
        private void commentsToLocp()
        {
            int totalComments = slashComments() + slashStarComments();
            int logicalLines = 0;
            int empty = myData.NumOfLines - myData.NonEmptyLines.Length;
            logicalLines = myData.NumOfLines - empty;
            double ratio = (double)totalComments / (double)logicalLines;
            myData.FileStatistics[8] = ratio.ToString("0.000");
        }
        private void logicalLoc()
        {
            int logicalLines = 0;
            int empty = myData.NumOfLines - myData.NonEmptyLines.Length;
            logicalLines = myData.NumOfLines - empty;
            myData.FileStatistics[9] = logicalLines.ToString();
            //logical Loc = SLOCP-EL-Whitespacelines-single brace lines
        }
        private void ratioWNW()
        {
            int whiteSpaceChars = myData.NumOfTabsChars + myData.NumOfSpaceChars;
            int nonWhiteSpaceChars = myData.FileText.Length - whiteSpaceChars;
            double ratio = (double)whiteSpaceChars / (double)nonWhiteSpaceChars;
            myData.FileStatistics[10] = ratio.ToString("0.000");
        }
        private void ELNEL()
        {
            int emptyLines = 0;
            double ELNEL = 0.0;
            emptyLines = myData.NumOfLines - myData.NonEmptyLines.Length;
            ELNEL = (double)emptyLines / (double)myData.NonEmptyLines.Length;
            myData.FileStatistics[11] = ELNEL.ToString("0.000");
        }
        private void tabsToChars()
        {
            int charCount = myData.FileText.Length;
            double ratio = (double)myData.NumOfTabsChars / (double)charCount;
            myData.FileStatistics[12] = ratio.ToString("0.000");
        }
        private void spacesToChars()
        {
            int charCount = myData.FileText.Length;
            double ratio = (double)myData.NumOfSpaceChars / (double)charCount;
            myData.FileStatistics[13] = ratio.ToString("0.000");
        }
        private void ElToCharss()
        {
            int charCount = myData.FileText.Length;
            int empty = myData.NumOfLines - myData.NonEmptyLines.Length;
            double ratio = (double)empty / (double)charCount;
            myData.FileStatistics[14] = ratio.ToString("0.000");
        }
        private void uppercaseLettersPercentage()
        {
            ulong totalUppercase = 0; double percentage = 0.0;
            for (int i = 0; i < myData.FileText.Length; i++)
            {
                if (char.IsUpper(myData.FileText, i))
                    totalUppercase++;
            }
            percentage = ((double)totalUppercase / (double)myData.FileText.Length) * 100;
            myData.FileStatistics[15] = percentage.ToString("0.000");
            
        }
        private void wordcounting()
        {
            b.Clear();
            ulong charsInAllWords = 0; ulong countwords = 0;
            double avgWordsPerLine = 0;
            string[] sep = new string[] { " ", "\t", ";", "," };
            string[] words = new string[] { string.Empty };
            ulong totalwords = 0;
           
            bool flag = false;
            foreach (string s in myData.NonEmptyLines)
            {
                words = s.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                foreach (string ss in words)
                {
                    foreach (char ch in ss)
                    {
                        if (char.IsLetterOrDigit(ch))
                        {
                            flag = true;
                        }
                    }
                    if (flag)
                    {
                        b.AppendLine(ss.Trim());
                        totalwords++;
                        charsInAllWords = charsInAllWords + (ulong)ss.Trim().Length;
                    }
                    flag = false;

                }
            }


            countwords = totalwords;
            int logicalLines = 0;
            int empty = myData.NumOfLines - myData.NonEmptyLines.Length;
            logicalLines = myData.NumOfLines - empty;
            avgWordsPerLine = (double)countwords / (double)myData.NonEmptyLines.Length;
           
            ulong avgCharsInWord = charsInAllWords / (ulong)countwords;
            myData.FileStatistics[16] = countwords.ToString();
            myData.FileStatistics[17] = avgWordsPerLine.ToString("0.000");
            myData.FileStatistics[18] = avgCharsInWord.ToString();
            
        }
        public void readSingleFile()
        {
            myData.FileText = File.ReadAllText(myData.NameOfFile);
            myData.AllTextLines = File.ReadAllLines(myData.NameOfFile);
            myData.NumOfLines = myData.AllTextLines.Length;
            myData.FileStatistics[0] = myData.NumOfLines.ToString();
            
        }
        public void writeAllStats(string filePath)
        {
            var csv = new StringBuilder();
            var newLine = string.Format("File,SLOC-P,SLOC-L,Chars,Statmnts,i_Iterator,Tabs,Spaces,AvgLineLength(C),AvgLineLength(W),Comments,Words,W/NW,(EL/Non-EL),Comments/Sloc-L,(Tabs/Chars),(Spaces/Chars),(EL/Chars),AvgWordLen(chars),Uppercase%");
            csv.AppendLine(newLine);
            //in your loop
            foreach (string file in myData.AllFileNames )
            {
                myData.AllTextLines = File.ReadAllLines(file);
                myData.NumOfLinesOfAllFiles = myData.NumOfLinesOfAllFiles + (ulong) myData.AllTextLines.Length;

                myData.FileText = File.ReadAllText(file);
                statistics();
                var newLine1 = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}", file, myData.FileStatistics[0],myData.FileStatistics[1],myData.FileStatistics[2],myData.FileStatistics[3],myData.FileStatistics[4],myData.FileStatistics[5],myData.FileStatistics[6],myData.FileStatistics[7],myData.FileStatistics[8],myData.FileStatistics[9],myData.FileStatistics[10],myData.FileStatistics[11],myData.FileStatistics[12],myData.FileStatistics[13],myData.FileStatistics[14],myData.FileStatistics[15],myData.FileStatistics[16],myData.FileStatistics[17],myData.FileStatistics[18]);
                csv.AppendLine(newLine1);
                
            }

            //after your loop
            File.WriteAllText(filePath, csv.ToString());
           
        }


        public static TreeViewItem CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeViewItem { Header = directoryInfo.Name };
            foreach (var directory in directoryInfo.GetDirectories())
                directoryNode.Items.Add(CreateDirectoryNode(directory));

            foreach (var file in directoryInfo.GetFiles("*.hpp"))
                directoryNode.Items.Add(new TreeViewItem { Header = file.Name });

            foreach (var file in directoryInfo.GetFiles("*.h"))
                directoryNode.Items.Add(new TreeViewItem { Header = file.Name });

            foreach (var file in directoryInfo.GetFiles("*.c"))
                directoryNode.Items.Add(new TreeViewItem { Header = file.Name });

            foreach (var file in directoryInfo.GetFiles("*.cpp"))
                directoryNode.Items.Add(new TreeViewItem { Header = file.Name });
            foreach (var file in directoryInfo.GetFiles("*.java"))
                directoryNode.Items.Add(new TreeViewItem { Header = file.Name });

            return directoryNode;

        }
        public void readFilesFromFolder()
        {
            string supportedExtensions = "*.h,*.cpp,*.c,*.java";

            StringBuilder fileNames = new StringBuilder();
            foreach (string Files in Directory.GetFiles(myData.SelectedFolderPath, "*.*", SearchOption.AllDirectories).Where(s => supportedExtensions.Contains(System.IO.Path.GetExtension(s).ToLower())))
            {
                fileNames.Append(Files).Append("\n");
                //do work here
            }
            string fileNamesString = fileNames.ToString();
            string[] separator = new string[] { "\n" };
            myData.AllFileNames = fileNamesString.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }
        public void countFile(ref ulong total_ch_p, ref ulong loctotal_p)
        {
            foreach (string file in myData.AllFileNames)
            {
                myData.NonEmptyLines = File.ReadAllLines(file);
                foreach (string s in myData.NonEmptyLines)
                {
                    total_ch_p = total_ch_p + (ulong)s.Length;
                }
                loctotal_p = loctotal_p + (ulong)myData.NonEmptyLines.Length;
            }

        }
        public void doCalculationsAndDisplay(string name)
        {
            myData.NameOfFile = name;

            statistics();
        }
        #endregion
    }
}
