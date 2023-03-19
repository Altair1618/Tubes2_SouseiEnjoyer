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
using Mazes;
using Microsoft.Win32;
using System.Diagnostics;
using IO;
using Views;

namespace src
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
        }

        public void openFileButton(object sender, RoutedEventArgs e)
        {
            Maze maze = new Maze();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                filename.Text = openFileDialog.SafeFileName;
                FileReader fileReader = new FileReader();
                maze = fileReader.read(openFileDialog.FileName);
                foreach (var row in maze.MazeLayout)
                {
                    foreach(var col in row)
                    {
                        Trace.Write(col.GetCategory() + " ");
                    }
                    Trace.WriteLine("");
                }
            }

            if (mazePanel.Children.Count > 0)
            {
                mazePanel.Children.RemoveAt(mazePanel.Children.Count - 1);
            }

            MazeView mazeGrid = new MazeView(maze);
            mazeGrid.Width = maze.GetCol() * 100;
            mazeGrid.Height = maze.GetRow() * 100;
            mazePanel.Children.Add(mazeGrid);
        }


    }
}
