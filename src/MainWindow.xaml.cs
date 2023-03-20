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
        private Maze maze;
        private MazeView mazeView;
        List<Tuple<Tuple<Tile, string>, List<Tile>>> result;
        public MainWindow()
        {
            this.maze = new Maze();
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
        }

        public void openFileButton(object sender, RoutedEventArgs e)
        {
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
                        Trace.Write(col.Category + " ");
                    }
                    Trace.WriteLine("");
                }
            }

            if (mazePanel.Children.Count > 0)
            {
                mazePanel.Children.RemoveAt(mazePanel.Children.Count - 1);
            }

            mazeView = new MazeView(maze);
            mazeView.Width = maze.GetCol() * 100;
            mazeView.Height = maze.GetRow() * 100;
            mazePanel.Children.Add(mazeView);
            search.IsEnabled = true;
        }

        public void searchButton(object sender, RoutedEventArgs e)
        {
            result = maze.BFS(maze.GetStartingTile());
            visualize.IsEnabled = true;
            nodes.Text += maze.BFSnodes.ToString();
            steps.Text += maze.BFSsteps.ToString();
            for (int i = 0; i < result.Count; i++)
            {
                route.Text += result[i].Item1.Item2;
            }
            mazeView.ShowResult(result);
        }

        public async void visualizeButton(object sender, RoutedEventArgs e)
        {
            search.IsEnabled = false;
            visualize.IsEnabled = false;
            mazeView.Reset();
            if (mazePanel.Children.Count > 0)
            {
                Grid mazeGrid = mazePanel.Children[0] as Grid;
                for (int i = 0; i < result.Count; i++)
                {
                    for (int j = 0; j < result[i].Item2.Count; j++)
                    {
                        Tuple<int, int> coordinate = maze.GetTileCoordinate(result[i].Item2[j]);
                        Trace.WriteLine(result[i].Item2[j].Id);
                        await Task.Delay(1000);
                        Border tileBorder = mazeGrid.Children[result[i].Item2[j].Id - 1] as Border;
                        if (j > 0)
                        {
                            Tuple<int, int> prevCoordinate = maze.GetTileCoordinate(result[i].Item2[j-1]);
                            Border prevTileBorder = mazeGrid.Children[result[i].Item2[j-1].Id - 1] as Border;
                            SolidColorBrush color = new SolidColorBrush(Colors.Yellow);
                            color.Opacity = 0.1 * (maze.MazeLayout[prevCoordinate.Item1][prevCoordinate.Item2].Visited);
                            prevTileBorder.Background = color;
                        }
                        maze.MazeLayout[coordinate.Item1][coordinate.Item2].Visited++;
                        tileBorder.Background = new SolidColorBrush(Colors.Aqua);
                    }
                }
            }

            replay.IsEnabled = true;
        }

        public void replayButton(object sender, RoutedEventArgs e)
        {
            visualize.IsEnabled = true;
            mazeView.Reset();
        }
    }
}
