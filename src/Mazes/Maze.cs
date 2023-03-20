using System;
using System.Collections.Generic;
using System.Linq;

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

        public string BFSRoute {
            get; private set;
        }
        public List<Tile> processRoute {
            get; private set;
        }
        private int treasureCount;
        
        private HashSet <Tile> visitedTreasures;
        
        // Constructor
        public Maze() {
            MazeLayout = new List<List<Tile>>();
            treasureCount = -1;
            BFSsteps = 0;
            BFSnodes = 0;
            visitedTreasures = new HashSet<Tile>();
            BFSRoute = "";
            processRoute = new List<Tile>();
            // processRoute.Add(GetStartingTile());
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

        public int GetFirstId()
        {
            return MazeLayout[0][0].Id;
        }

        public Tile GetTile(int row, int col)
        {
            return MazeLayout[row][col];
        }

        public void UpdateTreasureCount() {
            if (treasureCount == -1) {
                treasureCount = 0;
                foreach (var rowTile in MazeLayout) {
                    foreach (var tile in rowTile) {
                        if (tile.IsTreasure()) treasureCount++;
                    }
                }
            }
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
            return Tuple.Create(tileIdInMaze / GetCol(), tileIdInMaze % GetCol());
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
            path.Add(startingTile);
            processRoute.Add(startingTile);
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
                (int x, int y) = GetTileCoordinate(currentTile.Item1);
                
                // Console.WriteLine("Current Tile: " + currentTile.Item1.Id + " " + currentTile.Item2);
                path.Add(currentTile.Item1);

                if (currentTile.Item1.IsTreasure() && !this.visitedTreasures.Contains(currentTile.Item1)) {
                    this.visitedTreasures.Add(currentTile.Item1);
                    Tuple <Tuple<Tile, string>, List<Tile>> result = Tuple.Create(currentTile, path);
                    path.RemoveAt(path.Count-1);
                    path.RemoveAt(0);
                    path.RemoveAt(0);
                    processRoute = processRoute.Concat(path).ToList();
                    BFSsteps += currentTile.Item2.Length;
                    BFSRoute += currentTile.Item2;
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
            BFSsteps = 0;
            BFSnodes = 0;
            BFSRoute = "";
            processRoute = new List<Tile>();
            visitedTreasures = new HashSet<Tile>();
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

        public List<Tile> GetFinalPath() {
            List<Tile> ret = new List<Tile>();
            Tile start = GetStartingTile();
            ret.Add(start);
            (int x, int y)  = GetTileCoordinate(start);
            string Route = BFSRoute;
            foreach (var r in Route) {
                if (r == 'U') {
                    Tile newTile = MazeLayout[x - 1][y];
                    x--;
                    ret.Add(newTile);
                } else if (r == 'L') {
                    Tile newTile = MazeLayout[x][y - 1];
                    y--;
                    ret.Add(newTile);
                } else if (r == 'R') {
                    Tile newTile = MazeLayout[x][y+1];
                    y++;
                    ret.Add(newTile);
                } else {
                    Tile newTile = MazeLayout[x + 1][y];
                    x++;
                    ret.Add(newTile);
                }
            }

            return ret; 
        }

        public List<Tile> DFS() {
            Stack<Tile> dfsStack = new Stack<Tile>();
            var prevTile = new Dictionary<Tile, Tile>();
            int visitedTreasure = 0;

            List<Tile> retList = new List<Tile>();

            // Initiate state
            foreach (var tileList in MazeLayout) {
                foreach (var tile in tileList) {
                    prevTile[tile] = null!;
                }
            }

            dfsStack.Push(GetStartingTile());

            while (dfsStack.Count > 0) {
                // Initiate Search
                Tile currentTile = dfsStack.Pop();
                Console.WriteLine(GetTileCoordinate(currentTile));
                retList.Add(currentTile);
                if (currentTile.IsTreasure()) visitedTreasure++;
                (int row, int col) = GetTileCoordinate(currentTile);

                int cnt = 0;
                if (IsIndexValid(row + 1, col) && MazeLayout[row + 1][col].IsWalkable()) {
                    if (prevTile[MazeLayout[row + 1][col]] == null) {
                        dfsStack.Push(MazeLayout[row + 1][col]);
                        prevTile[MazeLayout[row + 1][col]] = currentTile;
                        cnt++;
                    }
                }

                if (IsIndexValid(row, col + 1) && MazeLayout[row][col + 1].IsWalkable()) {
                    if (prevTile[MazeLayout[row][col + 1]] == null) {
                        dfsStack.Push(MazeLayout[row][col + 1]);
                        prevTile[MazeLayout[row][col + 1]] = currentTile;
                        cnt++;
                    }
                }

                if (IsIndexValid(row, col - 1) && MazeLayout[row][col - 1].IsWalkable()) {
                    if (prevTile[MazeLayout[row][col - 1]] == null) {
                        dfsStack.Push(MazeLayout[row][col - 1]);
                        prevTile[MazeLayout[row][col - 1]] = currentTile;
                        cnt++;
                    }
                }

                if (IsIndexValid(row - 1, col) && MazeLayout[row - 1][col].IsWalkable()) {
                    if (prevTile[MazeLayout[row - 1][col]] == null) {
                        dfsStack.Push(MazeLayout[row - 1][col]);
                        prevTile[MazeLayout[row - 1][col]] = currentTile;
                        cnt++;
                    }
                }

                if (treasureCount == visitedTreasure) break;

                if (cnt == 0 && dfsStack.Count > 0) {
                    while (currentTile != prevTile[dfsStack.Peek()]) {
                        currentTile = prevTile[currentTile];
                        retList.Add(currentTile);
                    }
                }
            }
            dfsStack.Clear();

            return retList;
        }
    }
}
