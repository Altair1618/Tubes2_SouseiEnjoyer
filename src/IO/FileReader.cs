using Mazes;
using System;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;


namespace IO
{
    public class FileReader
    {
        public Maze read(string filePath)
        {
            Maze maze = new Maze();
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                // Read and display lines from the file until the end of
                // the file is reached.
                while ((line = sr.ReadLine()) != null)
                {
                    List<Tile> tiles = convertLine(line);
                    maze.MazeLayout.Add(tiles);
                }
            }
            return maze;
        }

        private List<Tile> convertLine(string line)
        {
            List<Tile> tiles = new List<Tile>();
            string[] strings = line.Split(null);
            foreach (string s in strings)
            {
                tiles.Add(new Tile(s.ToCharArray()[0]));
            }

            return tiles;
        }
    }
}