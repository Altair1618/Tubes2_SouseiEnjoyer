using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace Mazes {
    public class Maze {
        // Attributes
        public List <List <Tile>> MazeLayout {
            get; private set;
        }

        private int treasureCount;
        private HashSet <Tile> treasureTiles;

        // Constructor
        public Maze() {
            MazeLayout = new List<List<Tile>>();
            treasureCount = -1;
            treasureTiles = new HashSet<Tile>();
        }

<<<<<<< HEAD
        // Methods
        public int GetRow() {
            return MazeLayout.Count;
        }

        public int GetCol() {
            if (GetRow() == 0) {
                return 0;
            } else {
                return MazeLayout[0].Count;
            }
        }

        public Tile GetTile(int row, int col)
        {
            return MazeLayout[row][col];
        }

        private int GetTreasureCount() {
            if (treasureCount == -1) {
                treasureCount = 0;
                foreach (var rowTile in MazeLayout) {
                    foreach (var tile in rowTile) {
                        if (tile.IsTreasure()) treasureCount++;
                        }
                    }
                }

            return treasureCount;
        }

        private bool IsIndexValid(int row, int col) {
            return (row >= 0 && row < GetRow() && col >= 0 && col < GetCol());
        }

        public Tile GetStartingTile() {
            Tile ret = new Tile();
            bool found = false;

            foreach (var rowTile in MazeLayout) {
                foreach (var tile in rowTile) {
                    if (tile.IsStartingPoint()) {
                        ret = tile;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }

            return ret;
        }

        public Tuple<int, int> GetTileCoordinate(Tile tile) {
            int tileIdInMaze = tile.Id - MazeLayout[0][0].Id;
            return Tuple.Create(tileIdInMaze / GetRow(), tileIdInMaze % GetRow());
        }

        public static string GetInversedPath(string path) {
            string s = "";

            foreach (var c in path) {
                if (c == 'U') s += 'D';
                else if (c == 'L') s += 'R';
                else if (c == 'R') s += 'L';
                else s += 'U';
            }

            return s;
        }
    
        public Tuple<Tile, string> BFS(Tile startingTile) {
            // Queue to store tiles
            Queue<Tuple<Tile, string>> q = new Queue<Tuple<Tile, string>>();
            q.Enqueue(Tuple.Create(startingTile, ""));

            // Dictionary to store visited tiles
            Dictionary<Tile, bool> visited = new Dictionary<Tile, bool>();
            foreach (var rowTile in MazeLayout) {
                foreach (var tile in rowTile) {
                    visited.Add(tile, false);
                }
            }

            while (q.Count > 0) {
                Tuple<Tile, string> currentTile = q.Dequeue();
                Tuple<int, int> currTileCoor = GetTileCoordinate(currentTile.Item1);
                int x = currTileCoor.Item1;
                int y = currTileCoor.Item2;
                Console.WriteLine("Current Tile: " + currentTile.Item1.Id + " " + currentTile.Item2);

                if (currentTile.Item1.IsTreasure() && !this.treasureTiles.Contains(currentTile.Item1)) {
                    this.treasureTiles.Add(currentTile.Item1);
                    Console.WriteLine("Found: " + currentTile.Item1.Id + " " + currentTile.Item2 + " \n---------------------");
                    return currentTile;
                }

                // Up
                if (IsIndexValid(x - 1, y) && MazeLayout[x - 1][y].IsWalkable() && !visited[MazeLayout[x - 1][y]]) {
                    Tile newTile = MazeLayout[x - 1][y];
                    visited[newTile] = true;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "U"));
                }

                // Left
                if (IsIndexValid(x, y - 1) && MazeLayout[x][y - 1].IsWalkable() && !visited[MazeLayout[x][y - 1]]) {
                    Tile newTile = MazeLayout[x][y - 1];
                    visited[newTile] = true;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "L"));
                }

                // Right
                if (IsIndexValid(x, y + 1) && MazeLayout[x][y + 1].IsWalkable() && !visited[MazeLayout[x][y + 1]]) {
                    Tile newTile = MazeLayout[x][y + 1];
                    visited[newTile] = true;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "R"));
                }

                // Down
                if (IsIndexValid(x + 1, y) && MazeLayout[x + 1][y].IsWalkable() && !visited[MazeLayout[x + 1][y]]) {
                    Tile newTile = MazeLayout[x + 1][y];
                    visited[newTile] = true;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "D"));
                }
            }

            // No treasure found
            Console.WriteLine("No treasure found");
            return Tuple.Create(new Tile(), "");
        }

        public List< Tuple< Tile, string>> BFS (Tile startingTile, int trCount) {
            List <Tuple <Tile,string>> result = new List<Tuple<Tile, string>>();
            Tile start = GetStartingTile();
            for (int i = 0; i < trCount; i++) {

                Tuple <Tile, string> temp = BFS(start);
                result.Add(temp);
                start = temp.Item1;
            }
            return result;
        }

=======
>>>>>>> 175eb57 (add: maze and tile view)
    }
}
