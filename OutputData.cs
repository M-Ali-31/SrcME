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
    class OutputData
    {
        public string FileName
        {
            get;set;
        }
        public string Sloc_P
        {
            get;
            set;
        }
        public string Sloc_L
        {
            get;
            set;
        }
        public string Chars
        {
            get;set;
        }
        public string Statements
        {
            get;set;
        }
        public string i_Iterator
        {
            get;set;
        }
        public string Tabs
        {
            get;set;
        }
        public string Spaces
        {
            get;set;
        }
        public string AvgLineLength_C
        {
            get;set;
        }
        public string AvgLineLen_W
        {
            get;
            set;
        }
        public string Comments
        {
            get;set;
        }
        public string Words
        {
            get;
            set;
        }
        public string W_to_NW
        {
            get;
            set;
        }
        public string El_to_NonEl
        {
            get;
            set;
        }
        public string CommentsToSloc_L
        {
            get;set;
        }
        public string Tabs_to_Chars
        {
            get;set;
        }
        public string Spaces_to_Chars
        {
            get;set;
        }
        public string El_to_Chars
        {
            get;set;
        }
        public string AvgWordLen_C
        {
            get;
            set;
        }
        public string UppercasePercent
        {
            get;set;
        }
        
        public OutputData()
        {

        }
    }
}
