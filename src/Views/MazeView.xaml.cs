using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using Mazes;
namespace Views
{
    public partial class MazeView : Grid
    {
        private Maze maze;
        public MazeView(Maze maze)
        {
            this.maze = maze;
            InitializeComponent();
            this.Visualize();
        }

        public void Visualize()
        {
            for (int i = 0; i < maze.GetRow(); i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(100);

                mazeGrid.RowDefinitions.Add(row);
                for (int j = 0; j < maze.GetCol(); j++)
                {
                    // maze.maze[i][j].visualize(mazeGrid, i, j);
                    ColumnDefinition column = new ColumnDefinition();
                    column.Width = new GridLength(100);
                    mazeGrid.ColumnDefinitions.Add(column);

                    TileView tileView = new TileView(maze.GetTile(i, j));
                    mazeGrid.Children.Add(tileView);

                    Grid.SetRow(tileView, i);
                    Grid.SetColumn(tileView, j);
                }
            }

        }

        public void Reset()
        {
            
            for (int i = 0; i < maze.GetRow(); i++)
            {
                for (int j = 0; j < maze.GetCol(); j++)
                {
                    maze.MazeLayout[i][j].Visited = 0;
                    if (maze.MazeLayout[i][j].IsWalkable() || maze.MazeLayout[i][j].IsStartingPoint())
                    {
                        if (mazeGrid.Children[maze.MazeLayout[i][j].Id - maze.MazeLayout[0][0].Id] != null)
                        {
                            Border tileBorder = mazeGrid.Children[maze.MazeLayout[i][j].Id - maze.MazeLayout[0][0].Id] as Border;
                            tileBorder.Background = new SolidColorBrush(Colors.LightGray);
                        }            
                    }
                }
            }
        }

        public async Task ShowFinalPath(List<Tile> finalPath)
        {
            for (int i = 0; i < finalPath.Count; i++)
            {
                int tileId = finalPath[i].Id - maze.GetFirstId();
                Border tile = mazeGrid.Children[tileId] as Border;
                tile.Background = new SolidColorBrush(Colors.LightGreen);
                await Task.Delay(500);
            }
        }

        public async Task ShowProgress(List<Tile> progressTile)
        {
            this.Reset();
            for (int i = 0; i < progressTile.Count; i++)
            {
                Tuple<int, int> coordinate = maze.GetTileCoordinate(progressTile[i]);
                // Trace.WriteLine(result[i].Item2[j].Id);
                await Task.Delay(1000);
                int tileId = progressTile[i].Id - maze.GetFirstId();
                Border tile = mazeGrid.Children[tileId] as Border;
                if (i > 0)
                {
                    Tuple<int, int> prevCoordinate = maze.GetTileCoordinate(progressTile[i - 1]);
                    int prevTileId = progressTile[i - 1].Id - maze.GetFirstId();
                    Border prevTile = mazeGrid.Children[prevTileId] as Border;
                    SolidColorBrush color = new SolidColorBrush(Colors.Goldenrod);
                    color.Opacity = 0.15 * (maze.MazeLayout[prevCoordinate.Item1][prevCoordinate.Item2].Visited);
                    prevTile.Background = color;
                }
                maze.MazeLayout[coordinate.Item1][coordinate.Item2].Visited++;
                tile.Background = new SolidColorBrush(Colors.SkyBlue);
            }
        }
    }
}