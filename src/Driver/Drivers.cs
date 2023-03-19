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

    public class BFSDriver {
        static void Main(string[] args) {
            FileReader fileReader = new FileReader();
            Maze maze = fileReader.read("../../test/test2.txt");
            // Tuple<Tile, string> result = maze.BFS(maze.GetStartingTile());
            // List<Tuple<Tile, string>> results = maze.BFS(maze.GetStartingTile());
            List <Tuple <Tuple<Tile, string>, List<Tile>>> result = maze.BFS(maze.GetStartingTile());
            Console.WriteLine("Steps: " + maze.BFSsteps);
            Console.WriteLine("Nodes: " + maze.BFSnodes);
        }
    }
}