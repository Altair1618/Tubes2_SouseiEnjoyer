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
        List<Tile> processTiles;
        List<Tile> finalPath;
        private string algorithm;
        private int timeDelay = 100;
        public MainWindow()
        {
            this.maze = new Maze();
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
        }

        public void openFileButton(object sender, RoutedEventArgs e)
        {
            this.resetState();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
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
                    if (mazePanel.Children.Count > 0)
                    {
                        mazePanel.Children.RemoveAt(mazePanel.Children.Count - 1);
                    }

                    int tileSize = Math.Min(100, (int)mazePanel.ActualWidth / maze.GetRow());
                    mazeView = new MazeView(maze, tileSize);
                    mazeView.Width = maze.GetCol() * tileSize;
                    mazeView.Height = maze.GetRow() * tileSize;
                    mazePanel.Children.Add(mazeView);

                    if (algorithm != null)
                    {
                        search.IsEnabled = true;
                    }

                } catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                    MessageBox.Show("Invalid file format");
                    filename.Text = "-";
                }
            }

        }

        public async void searchButton(object sender, RoutedEventArgs e)
        {
            this.resetState();
            mazeView.Reset();
            // search.IsEnabled = true;
            if (algorithm == "BFS")
            {
                maze.BFS(maze.GetStartingTile());
                processTiles = maze.processRoute;
                finalPath = maze.GetFinalPath();
                nodes.Text += maze.BFSnodes.ToString();
                steps.Text += maze.BFSsteps.ToString();
                route.Text += maze.BFSRoute;
                visualize.IsEnabled = true;
            } else if (algorithm == "DFS")
            {
                processTiles = maze.DFS();
                finalPath = processTiles;
                visualize.IsEnabled = true;
                nodes.Text += processTiles.Count.ToString();
                steps.Text += processTiles.Distinct().Count().ToString();
            }

            search.IsEnabled = false;
        }

        public async void visualizeButton(object sender, RoutedEventArgs e)
        {
            mazeView.Reset();
            visualize.IsEnabled = false;
            inputfile.IsEnabled = false;

            await mazeView.ShowProgress(processTiles, timeDelay);
            await Task.Delay(100);
            await mazeView.ShowFinalPath(finalPath, timeDelay);

            visualize.IsEnabled = true;
            inputfile.IsEnabled = true;
            /*if (algorithm == "DFS")
            {
            } else if (algorithm == "BFS")
            {
                mazeView.Reset();
                if (mazePanel.Children.Count > 0)
                {
                    Grid mazeGrid = mazePanel.Children[0] as Grid;
                    for (int i = 0; i < result.Count; i++)
                    {
                        for (int j = 0; j < result[i].Item2.Count; j++)
                        {
                            if (i > 0 && j == 0)
                            {
                                continue;
                            }
                            Tuple<int, int> coordinate = maze.GetTileCoordinate(result[i].Item2[j]);
                            // Trace.WriteLine(result[i].Item2[j].Id);
                            await Task.Delay(1000);
                            Border tileBorder = mazeGrid.Children[result[i].Item2[j].Id - maze.MazeLayout[0][0].Id] as Border;
                            if (j > 0)
                            {
                                Tuple<int, int> prevCoordinate = maze.GetTileCoordinate(result[i].Item2[j-1]);
                                Border prevTileBorder = mazeGrid.Children[result[i].Item2[j-1].Id - maze.MazeLayout[0][0].Id] as Border;
                                SolidColorBrush color = new SolidColorBrush(Colors.Goldenrod);
                                color.Opacity = 0.15 * (maze.MazeLayout[prevCoordinate.Item1][prevCoordinate.Item2].Visited);
                                prevTileBorder.Background = color;
                            }
                            maze.MazeLayout[coordinate.Item1][coordinate.Item2].Visited++;
                            tileBorder.Background = new SolidColorBrush(Colors.SkyBlue);
                        }
                    }
                }
                replay.IsEnabled = true;
            }*/
        }

        public void replayButton(object sender, RoutedEventArgs e)
        {
            visualize.IsEnabled = true;
            mazeView.Reset();
        }

        public void setAlgorithm(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null)
            {
                algorithm = rb.Content.ToString();
                search.IsEnabled = true;
                visualize.IsEnabled = false;
                replay.IsEnabled = false;
            }
        }

        public void resetState()
        {
            route.Text = "Route: ";
            nodes.Text = "Nodes: ";
            steps.Text = "Steps: ";
            extime.Text = "Execution Time: ";
            search.IsEnabled = false;
            visualize.IsEnabled = false;
            replay.IsEnabled = false;
        }

        public void setTimeDelay(object sender, RoutedEventArgs e)
        {
            timeDelay = (int) timeDelaySlider.Value;
        }
    }
}
