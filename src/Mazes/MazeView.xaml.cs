using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace Mazes
{
    public partial class MazeView : Grid
    {
        public MazeView(Maze maze)
        {
            InitializeComponent();
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
    }
}