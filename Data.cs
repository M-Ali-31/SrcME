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
using System.Linq;
using System.Text;

namespace SrcME
{
    class Data
    {
        #region Fields(or Data Members)
        private string fileText;
        private string[] nonEmptyLines;
        string[] lineTerminator;
        private string[] allLines;
        private int numOfLines;
        private string[] fileStatistics;
        private string filename;
        private string csvfile;
        private int numOfTabsChars;
        private int numOfSpaceChars;
        private ulong totalWordsInFile;
        private ulong numOfLinesOfAllFiles;
        private string[] allFileNames;
        private string selectedFolderPath;
       
        #endregion

        #region Public Properties(or Getter/Setter methods)
        public string FileText
        {
            get
            {
                return fileText;
            }
            set
            {
                 fileText = value;
            }
        }
        public string[] NonEmptyLines
        {
            get
            {
                return nonEmptyLines;
            }
            set
            {
                nonEmptyLines = value;
            }
        }
        public string[] LineTerminator
        {
            get
            {
                return lineTerminator;
            }
            set
            {
                lineTerminator = value;
            }
        }
        public string[] AllTextLines
        {
            get
            {
                return allLines;
            }
            set
            {
                allLines = value;
            }
        }
        public int NumOfLines
        {
            get
            {
                return numOfLines;
            }
            set
            {
                numOfLines = value;
            }
        }
        public string[] FileStatistics
        {
            get
            {
                return fileStatistics;
            }
            set
            {
                fileStatistics = value;
            }
        }
        public string NameOfFile
        {
            get
            {
                return filename;
            }
            set
            {
                filename = value;
            }
        }
        public string CsvFileName
        {
            get
            {
                return csvfile;
            }
            set
            {
                csvfile = value;
            }
        }
        public int NumOfTabsChars
        {
            get
            {
                return numOfTabsChars;
            }
            set
            {
                numOfTabsChars = value;
            }
        }
        public int NumOfSpaceChars
        {
            get
            {
                return numOfSpaceChars;
            }
            set
            {
                numOfSpaceChars = value;
            }
        }
        public ulong TotalWordsInFile
        {
            get
            {
                return totalWordsInFile;
            }
            set
            {
                totalWordsInFile = value;
            }
        }
        public ulong NumOfLinesOfAllFiles
        {
            get
            {
                return numOfLinesOfAllFiles;
            }
            set
            {
                numOfLinesOfAllFiles = value;
            }
        }
        public string[] AllFileNames
        {
            get
            {
                return allFileNames;
            }
            set
            {
                allFileNames = value;
            }
        }
        public string SelectedFolderPath
        {
            get
            {
                return selectedFolderPath;
            }
            set
            {
                selectedFolderPath = value;
            }
        }
       
        #endregion

        #region Constructor
        public Data()
        {
            initializeAllFields();
        }
        #endregion

        #region Utility Methods
        private void initializeAllFields()
        {
            FileText = string.Empty;
            NonEmptyLines = new string[] { string.Empty };
            LineTerminator = new string[] { "\r\n", "\n", "\r" };
            AllTextLines = new string[] { string.Empty };
            NumOfLines = 0;
            FileStatistics = new string[19];
            NameOfFile = string.Empty;
            CsvFileName = string.Empty;
            NumOfTabsChars = 0;
            NumOfSpaceChars = 0;
            TotalWordsInFile = 0;
            NumOfLinesOfAllFiles = 0;
            AllFileNames = new string[] { string.Empty };
            SelectedFolderPath = string.Empty;
       
        }
        #endregion
    }
}
