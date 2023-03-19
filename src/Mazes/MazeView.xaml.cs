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
<<<<<<< HEAD
            for (int i = 0; i < maze.GetRow(); i++)
=======
            for (int i = 0; i < maze.maze.Count; i++)
>>>>>>> 175eb57 (add: maze and tile view)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(100);

                mazeGrid.RowDefinitions.Add(row);
<<<<<<< HEAD
                for (int j = 0; j < maze.GetCol(); j++)
=======
                for (int j = 0; j < maze.maze[i].Count; j++)
>>>>>>> 175eb57 (add: maze and tile view)
                {
                    // maze.maze[i][j].visualize(mazeGrid, i, j);
                    ColumnDefinition column = new ColumnDefinition();
                    column.Width = new GridLength(100);
                    mazeGrid.ColumnDefinitions.Add(column);

<<<<<<< HEAD
                    TileView tileView = new TileView(maze.GetTile(i, j));
=======
                    TileView tileView = new TileView(maze.maze[i][j]);
>>>>>>> 175eb57 (add: maze and tile view)
                    mazeGrid.Children.Add(tileView);

                    Grid.SetRow(tileView, i);
                    Grid.SetColumn(tileView, j);
                }
            }
        }
    }
}