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
        int startCount = 0;
        public Maze read(string filePath)
        {
            Maze maze = new Maze();
            startCount = 0;
            using (StreamReader sr = new StreamReader(filePath))
            {
                string? line;
                while ((line = sr.ReadLine()) != null)
                {
                    List<Tile> tiles = convertLine(line);
                    maze.MazeLayout.Add(tiles);
                }
            }
            if (startCount != 1)
            {
                throw new FileFormatException();
            }
            maze.UpdateTreasureCount();
            if (maze.treasureCount == 0)
            {
                throw new FileFormatException();
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
                if (tiles[tiles.Count - 1].IsStartingPoint()) { startCount++; }
            }

            return tiles;
        }
    }
}