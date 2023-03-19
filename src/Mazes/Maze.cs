using System;
using System.Collections.Generic;

namespace Mazes {
    public class Maze {
        // Attributes
        public List <List <Tile>> MazeLayout {
            get; private set;
        }
        public int BFSsteps {
            get; private set;
        }

        public int BFSnodes {
            get; private set;
        }

        private int treasureCount;
        private HashSet <Tile> treasureTiles;

        // Constructor
        public Maze() {
            MazeLayout = new List<List<Tile>>();
            treasureCount = -1;
            BFSsteps = 0;
            BFSnodes = 0;
            treasureTiles = new HashSet<Tile>();
        }

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
    
        /* BFSfind returns Tuple of a detail treasure and its process.*/
        public Tuple <Tuple<Tile, string>, List<Tile>> BFSfind(Tile startingTile) {
            // Queue to store 
            List<Tile> path = new List<Tile>();
            Queue<Tuple<Tile, string>> q = new Queue<Tuple<Tile, string>>();
            q.Enqueue(Tuple.Create(startingTile, ""));

            // Dictionary to store visited tiles
            Dictionary<Tile, bool> visited = new Dictionary<Tile, bool>();
            foreach (var rowTile in MazeLayout) {
                foreach (var tile in rowTile) {
                    visited.Add(tile, false);
                }
            }
            visited[startingTile] = true;
            int visitedCount = 0;
            while (q.Count > 0) {
                Tuple<Tile, string> currentTile = q.Dequeue();
                Tuple<int, int> currTileCoor = GetTileCoordinate(currentTile.Item1);
                int x = currTileCoor.Item1;
                int y = currTileCoor.Item2;
                Console.WriteLine("Current Tile: " + currentTile.Item1.Id + " " + currentTile.Item2);
                path.Add(currentTile.Item1);

                if (currentTile.Item1.IsTreasure() && !this.treasureTiles.Contains(currentTile.Item1)) {
                    this.treasureTiles.Add(currentTile.Item1);
                    Tuple <Tuple<Tile, string>, List<Tile>> result = Tuple.Create(currentTile, path);
                    BFSsteps += currentTile.Item2.Length;
                    BFSnodes += visitedCount;
                    Console.WriteLine("BFSnodes: " + BFSnodes);
                    return result;
                }

                // Up
                if (IsIndexValid(x - 1, y) && MazeLayout[x - 1][y].IsWalkable() && !visited[MazeLayout[x - 1][y]]) {
                    Tile newTile = MazeLayout[x - 1][y];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "U"));
                }

                // Left
                if (IsIndexValid(x, y - 1) && MazeLayout[x][y - 1].IsWalkable() && !visited[MazeLayout[x][y - 1]]) {
                    Tile newTile = MazeLayout[x][y - 1];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "L"));
                }

                // Right
                if (IsIndexValid(x, y + 1) && MazeLayout[x][y + 1].IsWalkable() && !visited[MazeLayout[x][y + 1]]) {
                    Tile newTile = MazeLayout[x][y + 1];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "R"));
                }

                // Down
                if (IsIndexValid(x + 1, y) && MazeLayout[x + 1][y].IsWalkable() && !visited[MazeLayout[x + 1][y]]) {
                    Tile newTile = MazeLayout[x + 1][y];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "D"));
                }
            }

            // No treasure found
            Tuple <Tuple<Tile, string>, List<Tile>> temp = new Tuple<Tuple<Tile, string>, List<Tile>>(Tuple.Create(new Tile(), ""), new List<Tile>());
            return temp;
        }

        public List <Tuple <Tuple<Tile, string>, List<Tile>>> BFS (Tile startingTile) {
            List <Tuple <Tuple<Tile, string>, List<Tile>>> result = new List<Tuple <Tuple<Tile, string>, List<Tile>>>();
            Tile start = GetStartingTile();
            bool finish = false;
            Console.WriteLine("Results: ");
            while  (!finish) {
                Tuple <Tuple<Tile, string>, List<Tile>> temp = BFSfind(start);
                
                if (temp.Item1.Item2 == "") {
                    finish = true;
                } else {
                    result.Add(temp);
                    start = temp.Item1.Item1;
                    Console.WriteLine("----------------\n Result: " + temp.Item1.Item1.Id  + " " + temp.Item1.Item2 + "\n----------------\n");
                }
            }
            /*print process second treasure*/
            // foreach (var path in result[1].Item2) {
            //     Console.WriteLine(path.Id);
            // }
            return result;
        }

    }
}
