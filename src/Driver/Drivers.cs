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
            // Console.WriteLine(maze.GetStartingTile().Id);
            List <Tuple <Tuple<Tile, string>, List<Tile>>> result = maze.BFS(maze.GetStartingTile());
            Console.WriteLine("Route: " + maze.BFSRoute);
            Console.WriteLine("Steps: " + maze.BFSsteps);
            Console.WriteLine("Nodes: " + maze.BFSnodes);
            List<Tile> finalPath = maze.GetFinalPath();
            foreach (var x in finalPath) {
                Console.Write(x.Id + " ");
            }
            Console.Write("\n");
            foreach(var x in maze.processRoute) {
                Console.Write(x.Id + " ");
            }
            
        }
    }

    public class DFSDriver {
        static void Main(string[] args) {
            Maze maze = new Maze();
            
            // Test 1 : RRRDDLL
            // string input = "KRRRXRXTXTRRXRXX";
            // int row = 4; int col = 4;

            // Test 2 : RRUUDDRRRUUDDLDDUULLLDD
            string input = "XXTXXTXXRXXRKRRRRRXRXXRXXTXXTX";
            int row = 5, col = 6;

            // Test 3 : RRRLLDD
            // string input = "KRRTXRXXXTXX";
            // int row = 3, col = 4;
            
            int index = 0;
            for (int i = 0; i < row; i++) {
                maze.MazeLayout.Add(new List<Tile>());
                for (int j = 0; j < col; j++) {
                    maze.MazeLayout[i].Add(new Tile(input[index++]));
                }
            }
            maze.UpdateTreasureCount();
            
            List<Tile> list = maze.DFS();
            
            Console.WriteLine("Output:");
            foreach (var item in list) {
                Console.WriteLine(maze.GetTileCoordinate(item));
            }
        }
    }
}