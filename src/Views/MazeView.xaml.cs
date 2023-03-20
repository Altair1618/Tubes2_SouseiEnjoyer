using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
                    if (maze.MazeLayout[i][j].IsWalkable())
                    {
                        if (mazeGrid.Children[maze.MazeLayout[i][j].Id - maze.MazeLayout[0][0].Id] != null)
                        {
                            Border tileBorder = mazeGrid.Children[maze.MazeLayout[i][j].Id - maze.MazeLayout[0][0].Id] as Border;
                            tileBorder.Background = new SolidColorBrush(Colors.DarkGray);
                        }            
                    }
                }
            }
        }

        public void ShowResult(List<Tuple<Tuple<Tile, string>, List<Tile>>> result)
        {
            for (int i = 0; i < result.Count; i++)
            {
                Border tileBorder = mazeGrid.Children[result[i].Item1.Item1.Id - 1] as Border;
                tileBorder.Background = new SolidColorBrush(Colors.LightGreen);
            }
        }
    }
}