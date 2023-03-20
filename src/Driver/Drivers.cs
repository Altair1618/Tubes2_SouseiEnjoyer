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
            for (int i = 0; i < result.Count; i++)
            {
                Console.WriteLine(i);
                Console.WriteLine(result[i].Item1.Item2);
                for (int j = 0; j < result[i].Item2.Count; j++)
                {
                    Console.Write(result[i].Item2[j].Id);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("Steps: " + maze.BFSsteps);
            Console.WriteLine("Nodes: " + maze.BFSnodes);
        }
    }
}