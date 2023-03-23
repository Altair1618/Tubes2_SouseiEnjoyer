using Mazes;
using IO;
using Graphs;
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
            // maze.BFS(maze.GetStartingTile());
            maze.TSP(maze.GetStartingTile());
            Console.WriteLine("\n--------TSP--------");
            Console.WriteLine("Route: " + maze.TSPRoute);
            List<Tile> finalPathTSP = maze.GetFinalPath(maze.TSPRoute);
            foreach (var x in finalPathTSP) {
                Console.Write(x.Id + " ");
            }
            Console.Write("\n");
            foreach(var x in maze.TSPprocessRoute) {
                Console.Write(x.Id + " ");
            }


            Console.WriteLine("\n--------BFS--------");
            Console.WriteLine("Route: " + maze.BFSRoute);
            List<Tile> finalPath = maze.GetFinalPath(maze.BFSRoute);
            foreach (var x in finalPath) {
                Console.Write(x.Id + " ");
            }
            Console.Write("\n");
            foreach(var x in maze.processRoute) {
                Console.Write(x.Id + " ");
            }

            // List<Tile> finalPathTSP = maze.GetFinalPathTSP();


            
        }
    }

    public class DFSDriver {
        static void Main(string[] args) {
            FileReader fileReader = new FileReader();
            Maze maze = fileReader.read("../../test/test2.txt");

            for (int i = 0; i < maze.GetRow(); i++) {
                for (int j = 0; j < maze.GetCol(); j++) {
                    Console.Write(maze.MazeLayout[i][j].Category + " ");
                }
                Console.WriteLine();
            }
            
            (List<Tile> list, List<Tile> path) = maze.DFS();
            
            Console.WriteLine("Output:");
            foreach (var item in list) {
                Console.WriteLine(maze.GetTileCoordinate(item));
            }
            Console.WriteLine("Path:");
            foreach (var item in path) {
                Console.WriteLine(maze.GetTileCoordinate(item));
            }

            Console.WriteLine(maze.GetMove(path));
        }
    }

    public class ShortestPathDriver {
        static void Main(string[] args) {
            Maze maze = new Maze();
            
            // Test 1
            // string input = "KRRRXRXTXTRRXRXX";
            // int row = 4; int col = 4;
            
            // Test 2
            // string input = "XXRXXTXXTXXRKRRRRRXRXXRXXTXXTX";
            // int row = 5, col = 6;

            // Test 3
            string input = "KTRXXTRXXTRXXTRXXTRXXTRXXTRXXTRXXTRXXTRXXTRX";
            int row = 11, col = 4;
            
            int index = 0;
            for (int i = 0; i < row; i++) {
                maze.MazeLayout.Add(new List<Tile>());
                for (int j = 0; j < col; j++) {
                    maze.MazeLayout[i].Add(new Tile(input[index++]));
                }
            }
            maze.UpdateTreasureCount();
            
            Graph graph = maze.ToWeightedGraph();
            Node startingNode = null!;
            foreach (var node in graph.Nodes) {
                if (node.Category == 'S') startingNode = node;
            }
        
            Console.WriteLine("Output:");
            var temp = graph.ShortestPath(false);
            Console.WriteLine(temp);
        }
    }
}