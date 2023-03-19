using Mazes;
using System;
using System.Collections.Generic;

namespace Drivers {
    public class MazeDriver {
        static void Main(string[] args) {
            Maze maze = new Maze();
            string input = "KRRRXRXTXTRRXRXX";
            int row = 4; int col = 4; int index = 0;
            
            for (int i = 0; i < row; i++) {
                maze.MazeLayout.Add(new List<Tile>());
                for (int j = 0; j < col; j++) {
                    maze.MazeLayout[i].Add(new Tile(input[index++]));
                }
            }

            Tuple<int, int> startingCoordinate = maze.GetTileCoordinate(maze.GetStartingTile());
            Console.WriteLine(startingCoordinate);
        }
    }
}