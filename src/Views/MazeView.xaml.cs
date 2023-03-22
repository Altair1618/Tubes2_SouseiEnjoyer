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
        public MazeView(Maze maze, int tileSize = 100)
        {
            this.maze = maze;
            InitializeComponent();
            this.Visualize(tileSize);
        }

        public void Visualize(int tileSize = 100)
        {
            for (int i = 0; i < maze.GetRow(); i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(tileSize);

                mazeGrid.RowDefinitions.Add(row);
                for (int j = 0; j < maze.GetCol(); j++)
                {
                    // maze.maze[i][j].visualize(mazeGrid, i, j);
                    ColumnDefinition column = new ColumnDefinition();
                    column.Width = new GridLength(tileSize);
                    mazeGrid.ColumnDefinitions.Add(column);

                    TileView tileView = new TileView(maze.GetTile(i, j), tileSize);
                    tileView.SetValue(Border.CornerRadiusProperty, new CornerRadius(tileSize / 10));
                    mazeGrid.Children.Add(tileView);

                    Grid.SetRow(tileView, i);
                    Grid.SetColumn(tileView, j);
                }
            }

        }

        public void Reset()
        {
            maze.ResetVisitedTiles();
            Border tileBorder = null!;
            for (int i = 0; i < maze.GetRow(); i++)
            {
                for (int j = 0; j < maze.GetCol(); j++)
                {
                    if (maze.MazeLayout[i][j].IsWalkable() || maze.MazeLayout[i][j].IsStartingPoint())
                    {
                        if (mazeGrid.Children[maze.MazeLayout[i][j].Id - maze.MazeLayout[0][0].Id] != null)
                        {
                            tileBorder = mazeGrid.Children[maze.MazeLayout[i][j].Id - maze.MazeLayout[0][0].Id] as Border;
                            tileBorder.Background = new SolidColorBrush(Colors.LightSteelBlue);
                            Border tileBorderChild = tileBorder.Child as Border;

                            if (tileBorderChild != null && tileBorderChild.Name == "imageOnImage")
                            {
                                tileBorder.Child = null;
                            }

                            if (tileBorderChild != null && tileBorderChild.Child != null)
                            {
                                tileBorderChild.Child = null;
                            }
                        }            
                    }
                }
            }
        }

        public async Task ShowFinalPath(List<Tile> finalPath, int timeDelay=100)
        {
            maze.ResetVisitedTiles();
            for (int i = 0; i < finalPath.Count; i++)
            {
                Tuple<int, int> coordinate = maze.GetTileCoordinate(finalPath[i]);
                maze.MazeLayout[coordinate.Item1][coordinate.Item2].Visited++;
                int tileId = finalPath[i].Id - maze.GetFirstId();
                TileView tile = mazeGrid.Children[tileId] as TileView;
                // tile.Background = new SolidColorBrush(Colors.LightGreen);
                if (i > 0)
                {
                    int prevTileId = finalPath[i - 1].Id - maze.GetFirstId();
                    Border prevTile = mazeGrid.Children[prevTileId] as Border;
                    Border prevTileChild = prevTile.Child as Border;
                    if (prevTileChild != null && prevTileChild.Name == "imageOnImage")
                    {
                        prevTile.Child = null;
                    }

                    if (prevTileChild.Child != null)
                    {
                        prevTileChild.Child = null;
                    }
                }
                SolidColorBrush color = new SolidColorBrush(Colors.LightGreen);
                color.Opacity = 0.3 * (maze.MazeLayout[coordinate.Item1][coordinate.Item2].Visited);
                tile.Background = color;
                tile.addImageOnImage("../../src/assets/kobo2.png");
                await Task.Delay(timeDelay);
            }
        }

        public async Task ShowProgress(List<Tile> progressTile, int timeDelay=100)
        {
            this.Reset();
            for (int i = 0; i < progressTile.Count; i++)
            {
                Tuple<int, int> coordinate = maze.GetTileCoordinate(progressTile[i]);
                // Trace.WriteLine(result[i].Item2[j].Id);
                await Task.Delay(timeDelay);
                int tileId = progressTile[i].Id - maze.GetFirstId();
                Border tile = mazeGrid.Children[tileId] as Border;
                if (i > 0)
                {
                    Tuple<int, int> prevCoordinate = maze.GetTileCoordinate(progressTile[i - 1]);
                    int prevTileId = progressTile[i - 1].Id - maze.GetFirstId();
                    Border prevTile = null!;
                    prevTile = mazeGrid.Children[prevTileId] as Border;
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