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
    public static class CustomCommands
    {
        public static readonly RoutedUICommand OpenFile = new RoutedUICommand
                (
                        "OpenFile",
                        "OpenFile",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                                {
                                        new KeyGesture(Key.O, ModifierKeys.Control)
                                }
                );
        public static readonly RoutedUICommand OpenFolder = new RoutedUICommand
                (
                        "OpenFolder",
                        "OpenFolder",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                                {
                                        new KeyGesture(Key.B, ModifierKeys.Control)
                                }
                );
        public static readonly RoutedUICommand ProcessFolder = new RoutedUICommand
                (
                        "ProcessFolder",
                        "ProcessFolder",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                                {
                                        new KeyGesture(Key.F, ModifierKeys.Control)
                                }
                );

        public static readonly RoutedUICommand About = new RoutedUICommand
                (
                        "About",
                        "About",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                                {
                                        new KeyGesture(Key.A, ModifierKeys.Control)
                                }
                );

        public static readonly RoutedUICommand Exit = new RoutedUICommand
                (
                        "Exit",
                        "Exit",
                        typeof(CustomCommands),
                        new InputGestureCollection()
                                {
                                        new KeyGesture(Key.F4, ModifierKeys.Alt)
                                }
                );
        //Define more commands here, just like the one above
    }
}
