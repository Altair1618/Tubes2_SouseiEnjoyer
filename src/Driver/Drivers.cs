using Mazes;
using IO;
using System;
using System.Collections.Generic;

namespace Drivers {
    public class MazeDriver {
        static void Main(string[] args) {
            FileReader fileReader = new FileReader();
            Maze maze = fileReader.read("../../test/test1.txt");

            Tuple<int, int> startingCoordinate = maze.GetTileCoordinate(maze.GetStartingTile());
            Console.WriteLine(startingCoordinate);
        }
    }
}