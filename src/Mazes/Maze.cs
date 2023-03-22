using System;
using System.Collections.Generic;
using System.Linq;
// using Graphs;

namespace Mazes {
    public class Maze {
        // Attributes
        public List <List <Tile>> MazeLayout {
            get; private set;
        }

        public string BFSRoute {
            get; private set;
        }
        public List<Tile> processRoute {
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

            foreach (var c in path.Reverse()) {
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
                    BFSRoute += currentTile.Item2;
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

        public Tile BFS (Tile startingTile) {
            // restart
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
            return start;
        }

        public void TSP (Tile startingTile) {
            // restart
            Tile lastTreasure = BFS(startingTile);
            TSPRoute = BFSRoute;
            TSPprocessRoute = processRoute;
            TSPfind(lastTreasure);
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
                    TSPRoute += currentTile.Item2;
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

        public string GetMove(List<Tile> path) {
            string retString = "";

            for (int i = 1; i < path.Count(); i++) {
                (int currRow, int currCol) = GetTileCoordinate(path[i]);
                (int prevRow, int prevCol) = GetTileCoordinate(path[i - 1]);

                if (currRow == prevRow && currCol == prevCol + 1) {
                    retString += "R";
                } else if (currRow == prevRow && currCol == prevCol - 1) {
                    retString += "L";
                } else if (currRow == prevRow - 1 && currCol == prevCol) {
                    retString += "U";
                } else {
                    retString += "D";
                }
            }

            return retString;
        }

        private class TileGraphState {
            // Attributes
            public Tile PrevTile;
            public Tile LastNode;
            public string Path;

            // Constructor
            public TileGraphState(Tile prev, Tile lastNode, string path) {
                PrevTile = prev;
                LastNode = lastNode;
                Path = path;
            }
        }

        public Graph ToWeightedGraph() {
            Graph graph = new Graph();

            Stack<Tile> queueStack = new Stack<Tile>();
            Dictionary<Tile, Node> mapNodeTile = new Dictionary<Tile, Node>();
            Dictionary<Tile, TileGraphState> state = new Dictionary<Tile, TileGraphState>();

            // Initiate state
            foreach (var tileList in MazeLayout) {
                foreach (var tile in tileList) {
                    state[tile] = new TileGraphState(null!, null!, "");
                }
            }

            queueStack.Push(GetStartingTile());
            Node start = new Node('S');
            mapNodeTile[GetStartingTile()] = start;
            graph.AddNode(start);
            state[GetStartingTile()] = new TileGraphState(null!, GetStartingTile(), "");
            while (queueStack.Count > 0) {
                // Initiate Search
                Tile currentTile = queueStack.Pop();
                (int row, int col) = GetTileCoordinate(currentTile);
                
                if (currentTile.IsTreasure()) {
                    Node tempNode = new Node('T');
                    mapNodeTile[currentTile] = tempNode;
                    graph.AddNode(tempNode);
                    graph.AddEdge(mapNodeTile[state[currentTile].LastNode], tempNode, state[currentTile].Path);
                    state[currentTile] = new TileGraphState(state[currentTile].PrevTile, currentTile, "");
                }

                int cnt = 0;
                if (IsIndexValid(row + 1, col) && (MazeLayout[row + 1][col].IsWalkable() || MazeLayout[row + 1][col].IsStartingPoint())) cnt++;
                if (IsIndexValid(row, col + 1) && (MazeLayout[row][col + 1].IsWalkable() || MazeLayout[row][col + 1].IsStartingPoint())) cnt++;
                if (IsIndexValid(row, col - 1) && (MazeLayout[row][col - 1].IsWalkable() || MazeLayout[row][col - 1].IsStartingPoint())) cnt++;
                if (IsIndexValid(row - 1, col) && (MazeLayout[row - 1][col].IsWalkable() || MazeLayout[row - 1][col].IsStartingPoint())) cnt++;

                if (cnt != 2 && !(currentTile.IsTreasure() || currentTile.IsStartingPoint())) {
                    Node tempNode = new Node('N');
                    mapNodeTile[currentTile] = tempNode;
                    graph.AddNode(tempNode);
                    graph.AddEdge(mapNodeTile[state[currentTile].LastNode], tempNode, state[currentTile].Path);
                    state[currentTile] = new TileGraphState(state[currentTile].PrevTile, currentTile, "");
                }
                
                Tile accessedTile;
                
                if (IsIndexValid(row + 1, col) ) {
                    accessedTile = MazeLayout[row + 1][col];
                    if (accessedTile.IsWalkable()) {
                        if (state[accessedTile].PrevTile == null) {
                            state[accessedTile].Path = state[currentTile].Path + "D";
                            state[accessedTile].PrevTile = currentTile;
                            state[accessedTile].LastNode = state[currentTile].LastNode;
                            queueStack.Push(accessedTile);
                        } else if (mapNodeTile.ContainsKey(accessedTile) && accessedTile != state[currentTile].PrevTile) {
                            graph.AddEdge(mapNodeTile[state[currentTile].LastNode], mapNodeTile[accessedTile], state[currentTile].Path + "D");
                        }
                    }
                }

                if (IsIndexValid(row, col + 1)) {
                    accessedTile = MazeLayout[row][col + 1];
                    if (accessedTile.IsWalkable()) {
                        if (state[accessedTile].PrevTile == null) {
                            state[accessedTile].Path = state[currentTile].Path + "R";
                            state[accessedTile].PrevTile = currentTile;
                            state[accessedTile].LastNode = state[currentTile].LastNode;
                            queueStack.Push(accessedTile);
                        } else if (mapNodeTile.ContainsKey(accessedTile) && accessedTile != state[currentTile].PrevTile) {
                            graph.AddEdge(mapNodeTile[state[currentTile].LastNode], mapNodeTile[accessedTile], state[currentTile].Path + "R");
                        }
                    }
                }

                if (IsIndexValid(row, col - 1)) {
                    accessedTile = MazeLayout[row][col - 1];
                    if (accessedTile.IsWalkable()) {
                        if (state[accessedTile].PrevTile == null) {
                            state[accessedTile].Path = state[currentTile].Path + "L";
                            state[accessedTile].PrevTile = currentTile;
                            state[accessedTile].LastNode = state[currentTile].LastNode;
                            queueStack.Push(accessedTile);
                        } else if (mapNodeTile.ContainsKey(accessedTile) && accessedTile != state[currentTile].PrevTile) {
                            graph.AddEdge(mapNodeTile[state[currentTile].LastNode], mapNodeTile[accessedTile], state[currentTile].Path + "L");
                        }
                    }
                }

                if (IsIndexValid(row - 1, col)) {
                    accessedTile = MazeLayout[row - 1][col];
                    if (accessedTile.IsWalkable()) {
                        if (state[accessedTile].PrevTile == null) {
                            state[accessedTile].Path = state[currentTile].Path + "U";
                            state[accessedTile].PrevTile = currentTile;
                            state[accessedTile].LastNode = state[currentTile].LastNode;
                            queueStack.Push(accessedTile);
                        } else if (mapNodeTile.ContainsKey(accessedTile) && accessedTile != state[currentTile].PrevTile) {
                            graph.AddEdge(mapNodeTile[state[currentTile].LastNode], mapNodeTile[accessedTile], state[currentTile].Path + "U");
                        }
                    }
                }
            }
            queueStack.Clear();

            return graph;
        }

        public List<Tile> PathToList(Tile start, string path) {
            List<Tile> ret = new List<Tile>();

            ret.Add(start);
            Tile currentTile = start;
            foreach (var c in path) {
                (int row, int col) = GetTileCoordinate(currentTile);

                if (c == 'U') currentTile = MazeLayout[row - 1][col];
                else if (c == 'L') currentTile = MazeLayout[row][col - 1];
                else if (c == 'R') currentTile = MazeLayout[row][col + 1];
                else currentTile = MazeLayout[row + 1][col];

                ret.Add(currentTile);
            }

            return ret;
        }
    }
}
