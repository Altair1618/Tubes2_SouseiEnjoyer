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
using Graphs;
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
        private Maze maze = new Maze();
        private MazeView? mazeView;
        private List<Tile>? processTiles;
        private List<Tile>? finalPath;
        private string? resultRoute;
        private string? algorithm;
        private double? elapsedTime;
        private int timeDelay = 100;

        public MainWindow()
        {
            this.WindowState = WindowState.Maximized;
            InitializeComponent();
            Uri iconUri = new Uri("../../src/assets/treasure.ico", UriKind.Relative);
            window.Icon = BitmapFrame.Create(iconUri);
            Border b2 = new Border();
            ImageBrush image = new ImageBrush();
            BitmapImage myBitmapImage = new BitmapImage();

            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri("../../src/assets/kobo.png", UriKind.Relative);

            myBitmapImage.DecodePixelWidth = 200;
            myBitmapImage.EndInit();

            image.ImageSource = myBitmapImage;
            b2.Background = image;
            b2.HorizontalAlignment = HorizontalAlignment.Left;
            b2.VerticalAlignment = VerticalAlignment.Bottom;
            b2.Width = 200;
            b2.Height = 200;
            gridBorder.Child = b2;
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

                    int tileSize = Math.Min(100, (int)mazePanel.ActualWidth / maze.GetCol());
                    tileSize = Math.Min(tileSize, (int)System.Windows.SystemParameters.PrimaryScreenHeight / maze.GetRow() - 8);
                    tileSize = Math.Max(tileSize, 20);
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
            Stopwatch stopwatch = new Stopwatch();
            if (mazeView != null) mazeView.Reset();

            if (algorithm == "BFS")
            {
                stopwatch.Start();
                maze.BFS(maze.GetStartingTile());
                stopwatch.Stop();
                processTiles = maze.processRoute;
                finalPath = maze.GetFinalPath(maze.BFSRoute);
                resultRoute = maze.BFSRoute;
                elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
            } else if (algorithm == "DFS")
            {
                stopwatch.Start();
                processTiles = maze.DFS();
                stopwatch.Stop();
                finalPath = processTiles;
                resultRoute = maze.GetMove(finalPath);
                elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
            } else if (algorithm == "TSP")
            {
                stopwatch.Start();
                maze.TSP(maze.GetStartingTile());
                stopwatch.Stop();
                processTiles = maze.TSPprocessRoute;
                finalPath = maze.GetFinalPath(maze.TSPRoute);
                resultRoute = maze.TSPRoute;
                elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
            } else if (algorithm == "Shortest Path")
            {
                getAndUpdateShortestPath(false);   
            } else if (algorithm == "Shortest Path (TSP)")
            {
                getAndUpdateShortestPath(true);
            }

            await updateResult();
            visualize.IsEnabled = true;
            search.IsEnabled = false;
        }

        private void getAndUpdateShortestPath(bool isTSP)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Graph graph = maze.ToWeightedGraph();
            resultRoute = graph.ShortestPath(isTSP);
            stopwatch.Stop();
            processTiles = maze.PathToList(maze.GetStartingTile(), resultRoute);
            finalPath = processTiles;
            elapsedTime = stopwatch.Elapsed.TotalMilliseconds;
        }

        private async Task updateResult()
        {
            if (processTiles != null) nodes.Text += (processTiles.Count).ToString();
            if (finalPath != null) steps.Text += (finalPath.Count - 1).ToString();
            if (resultRoute != null) route.Text += resultRoute;
            if (elapsedTime != null)
            {
                extime.Text += elapsedTime.ToString();
                extime.Text += " ms";
            }
            if (finalPath != null) await mazeView.ShowFinalPath(finalPath);
        }

        public async void visualizeButton(object sender, RoutedEventArgs e)
        {
            if (mazeView != null && processTiles != null && finalPath != null) { 
                mazeView.Reset();
                visualize.IsEnabled = false;
                inputfile.IsEnabled = false;

                await mazeView.ShowProgress(processTiles, timeDelay);
                await Task.Delay(timeDelay);
                await mazeView.ShowFinalPath(finalPath, timeDelay);

                visualize.IsEnabled = true;
                inputfile.IsEnabled = true;
            }
        }

        public void replayButton(object sender, RoutedEventArgs e)
        {
            if (mazeView != null)
            {
                visualize.IsEnabled = true;
                mazeView.Reset();
            }
        }

        public void setAlgorithm(object sender, RoutedEventArgs e)
        {
            RadioButton rb = null!;
            rb = sender as RadioButton;
            algorithm = rb.Content.ToString();
            if (mazeView != null)
            {
                search.IsEnabled = true;
            }
            visualize.IsEnabled = false;
        }

        public void resetState()
        {
            route.Text = "Route: ";
            nodes.Text = "Nodes: ";
            steps.Text = "Steps: ";
            extime.Text = "Execution Time: ";
            search.IsEnabled = false;
            visualize.IsEnabled = false;
            //replay.IsEnabled = false;
        }

        public void setTimeDelay(object sender, RoutedEventArgs e)
        {
            timeDelay = (int) timeDelaySlider.Value;
        }
    }
}
