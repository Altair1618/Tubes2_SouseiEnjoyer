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
        public int TSPsteps {
            get; private set;
        }
        public int TSPnodes {
            get; private set;
        }
        public string TSPRoute {
            get; private set;
        }
        public List<Tile> TSPprocessRoute {
            get; private set;
        }
        private int treasureCount;
        
        private HashSet <Tile> visitedTreasures;
        
        // Constructor
        public Maze() {
            MazeLayout = new List<List<Tile>>();
            visitedTreasures = new HashSet<Tile>();
            treasureCount = -1;
            BFSsteps = 0;
            TSPsteps = 0;
            BFSnodes = 0;
            TSPnodes = 0;
            BFSRoute = "";
            TSPRoute = "";
            processRoute = new List<Tile>();
            TSPprocessRoute = new List<Tile>();
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

        public void ResetVisitedTiles()
        {
            foreach (var rowTile in MazeLayout)
            {
                foreach (var tile in rowTile)
                {
                    tile.Visited = 0;
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
    
        public Tuple<Tile, string> BFSfind(Tile startingTile) {
            // initialize
            List<Tile> path = new List<Tile>();
            processRoute.Add(startingTile);

            // Queue to store 
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
                path.Add(currentTile.Item1);

                if (currentTile.Item1.IsTreasure() && !this.visitedTreasures.Contains(currentTile.Item1)) {
                    this.visitedTreasures.Add(currentTile.Item1);
                    // updating attributes
                    path.RemoveAt(path.Count-1);
                    path.RemoveAt(0);
                    processRoute = processRoute.Concat(path).ToList();
                    BFSsteps += currentTile.Item2.Length;
                    BFSRoute += currentTile.Item2;
                    BFSnodes += visitedCount;
                    return currentTile;
                }

                // Up
                if (IsIndexValid(x - 1, y) && MazeLayout[x - 1][y].isWalkable() && !visited[MazeLayout[x - 1][y]]) {
                    Tile newTile = MazeLayout[x - 1][y];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "U"));
                }

                // Left
                if (IsIndexValid(x, y - 1) && MazeLayout[x][y - 1].isWalkable() && !visited[MazeLayout[x][y - 1]]) {
                    Tile newTile = MazeLayout[x][y - 1];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "L"));
                }

                // Right
                if (IsIndexValid(x, y + 1) && MazeLayout[x][y + 1].isWalkable() && !visited[MazeLayout[x][y + 1]]) {
                    Tile newTile = MazeLayout[x][y + 1];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "R"));
                }

                // Down
                if (IsIndexValid(x + 1, y) && MazeLayout[x + 1][y].isWalkable() && !visited[MazeLayout[x + 1][y]]) {
                    Tile newTile = MazeLayout[x + 1][y];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "D"));
                }
            }

            // No treasure found
            Tuple <Tile, string> temp = Tuple.Create(new Tile(), "");
            return temp;
        }

        public void BFS (Tile startingTile) {
            // restart
            BFSsteps = 0;
            BFSnodes = 0;
            BFSRoute = "";
            processRoute = new List<Tile>();
            visitedTreasures = new HashSet<Tile>();
            Tuple <Tile, string> temp = Tuple.Create(new Tile(), "");
            Tile start = GetStartingTile();
            bool finish = false;
            while  (!finish) {
                temp = BFSfind(start);
                if (temp.Item2 == "") {
                    finish = true; 
                } else {
                    start = temp.Item1;
                    Console.WriteLine("temp: " + temp.Item1.Id);
                }
            }
            Console.WriteLine("last temp: " + temp.Item1.Id);

            TSPnodes = BFSnodes;
            TSPsteps = BFSsteps;
            TSPRoute = BFSRoute;
            TSPprocessRoute = processRoute;
            TSPfind(start);

        }

        public Tuple<Tile, string> TSPfind(Tile startingTile) {
            // initialize
            List<Tile> path = new List<Tile>();
            processRoute.Add(startingTile);

            // Queue to store 
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
                path.Add(currentTile.Item1);

                if (currentTile.Item1.IsStartingPoint()) {
                    // updating attributes
                    path.RemoveAt(path.Count-1);
                    path.RemoveAt(0);
                    TSPprocessRoute = TSPprocessRoute.Concat(path).ToList();
                    TSPsteps += currentTile.Item2.Length;
                    Console.WriteLine("TSPsteps: " + TSPsteps);
                    Console.WriteLine("TSP+: " + currentTile.Item2.Length);
                    TSPRoute += currentTile.Item2;
                    TSPnodes += visitedCount;
                    return currentTile;
                }

                // Up
                if (IsIndexValid(x - 1, y) && MazeLayout[x - 1][y].isWalkable() && !visited[MazeLayout[x - 1][y]]) {
                    Tile newTile = MazeLayout[x - 1][y];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "U"));
                }

                // Left
                if (IsIndexValid(x, y - 1) && MazeLayout[x][y - 1].isWalkable() && !visited[MazeLayout[x][y - 1]]) {
                    Tile newTile = MazeLayout[x][y - 1];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "L"));
                }

                // Right
                if (IsIndexValid(x, y + 1) && MazeLayout[x][y + 1].isWalkable() && !visited[MazeLayout[x][y + 1]]) {
                    Tile newTile = MazeLayout[x][y + 1];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "R"));
                }

                // Down
                if (IsIndexValid(x + 1, y) && MazeLayout[x + 1][y].isWalkable() && !visited[MazeLayout[x + 1][y]]) {
                    Tile newTile = MazeLayout[x + 1][y];
                    visited[newTile] = true;
                    visitedCount++;
                    q.Enqueue(Tuple.Create(newTile, currentTile.Item2 + "D"));
                }
            }

            // No treasure found
            Tuple <Tile, string> temp = Tuple.Create(new Tile(), "");
            return temp;
        }

        // public void TSP () {
            
        // }

        public List<Tile> GetFinalPath(string route) {
            List<Tile> ret = new List<Tile>();
            Tile start = GetStartingTile();
            ret.Add(start);
            (int x, int y)  = GetTileCoordinate(start);
            foreach (var r in route) {
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
