using Mazes;
using System;
using System.Collections.Generic;

namespace Drivers {
    public class MazeDriver {
        static void Main(string[] args) {
            Maze maze = new Maze();
            
            for (int i = 0; i < 4; i++) {
                List<Tile> temp = new List<Tile>();
                for (int j = 0; j < 3; j++) {
                    temp.Add(new Tile());
                }
                maze.maze.Add(temp);
            }

            foreach (var row in maze.maze) {
                foreach (var col in row) {
                    Console.Write(col.Id + " ");
                }
                Console.WriteLine("");
            }
        }
    }
}