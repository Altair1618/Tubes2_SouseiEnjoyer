using Mazes;
using System.Collections.Generic;

namespace Graphs {
    public class Graph {
        // Attributes
        public List<Node> Nodes {
            get; private set;
        }

        // Constructor
        public Graph() {
            Nodes = new List<Node>();
        }

        // Methods
        // Adding Node
        public void AddNode(params Node[] nodes) {
            foreach (Node node in nodes) {
                Nodes.Add(node);
            }
        }

        // Adding Edge
        public void AddEdge(Node src, Node dest, string path) {
            src.AddEdge(dest, path);
            dest.AddEdge(src, Maze.GetInversedPath(path));
        }

        public Dictionary<Node, string> GetShortestTree(Node start) {
            // Mendapatkan Shortest Tree dengan Dijkstra
            var ret = new Dictionary<Node, string>();

            PriorityQueue<Node, int> pqueue = new PriorityQueue<Node, int>();
            List<Node> hasChecked = new List<Node>();
            
            ret[start] = "";
            pqueue.Enqueue(start, 0);
            while (pqueue.Count > 0) {
                Node checkNode = pqueue.Dequeue();

                if (hasChecked.Contains(checkNode)) {
                    continue;
                } else {
                    hasChecked.Add(checkNode);
                }
                
                foreach (var edge in checkNode.Edges) {
                    if (!hasChecked.Contains(edge.Dest)) {
                        if (ret.ContainsKey(edge.Dest)) {
                            if ((ret[checkNode] + edge.Path).Length < ret[edge.Dest].Length) {
                                ret[edge.Dest] = ret[checkNode] + edge.Path;
                                pqueue.Enqueue(edge.Dest, ret[edge.Dest].Length);
                            }
                        } else {
                            ret[edge.Dest] = ret[checkNode] + edge.Path;
                            pqueue.Enqueue(edge.Dest, ret[edge.Dest].Length);
                        }
                    }
                }
            }

            return ret;
        }

        public Graph ToReducedGraph() {
            // Generate Complete Graph dari Graph Maze yang hanya terdiri atas Treasure dan Start Node
            // Dilakukan dengan Dijkstra's Algorithm yang dimodifikasi
            Graph newGraph = new Graph();
            
            Dictionary<Node, Node> mapNewNode = new Dictionary<Node, Node>();
            foreach (var node in Nodes) {
                if (node.Category != 'N') {
                    Node tempNode = new Node(node.Category);
                    newGraph.AddNode(tempNode);
                    mapNewNode[node] = tempNode;
                }
            }

            List<Node> addedNodes = new List<Node>();
            foreach (var node in Nodes) {
                if (node.Category != 'N') {
                    addedNodes.Add(node);
                    var minimumDistance = GetShortestTree(node);

                    foreach (var item in minimumDistance) {
                        if (!addedNodes.Contains(item.Key) && item.Key.Category != 'N') {
                            newGraph.AddEdge(mapNewNode[node], mapNewNode[item.Key], item.Value);
                        }
                    }
                }
            }

            // foreach (var node in newGraph.Nodes) {
            //     Console.WriteLine(node.Id);
            //     foreach (var edge in node.Edges) {
            //         Console.WriteLine(edge.Src.Id + " " + edge.Dest.Id + " " + edge.Path);
            //     }
            // }

            return newGraph;
        }

        public string ShortestPath(bool isTSP) {
            Graph ReducedGraph = ToReducedGraph();
            Node startingNode = null!;

            foreach (var node in ReducedGraph.Nodes) {
                if (node.Category == 'S') {
                    startingNode = node;
                }
            }

            if (isTSP) return visitNodeTSP(startingNode, new List<Node>(), ReducedGraph.Nodes.Count);
            else return visitNode(startingNode, new List<Node>());
        }

        public string visitNode(Node src, List<Node> visited) {
            string shortest = "";

            foreach (var edge in src.Edges) {
                if (!visited.Contains(edge.Dest)) {
                    visited.Add(src);
                    
                    string temp = edge.Path + visitNode(edge.Dest, visited);
                    if (shortest == "" || temp.Length < shortest.Length) {
                        shortest = temp;
                    }
                    
                    visited.Remove(src);
                }
            }

            return shortest;
        }

        public string visitNodeTSP(Node src, List<Node> visited, int nodeCount) {
            string shortest = "";

            if (visited.Count == nodeCount - 1) {
                foreach (var edge in src.Edges) {
                    if (edge.Dest.Category == 'S') shortest += edge.Path;
                }
            } else {
                foreach (var edge in src.Edges) {
                    if (!visited.Contains(edge.Dest)) {
                        visited.Add(src);
                        
                        string temp = edge.Path + visitNodeTSP(edge.Dest, visited, nodeCount);
                        if (shortest == "" || temp.Length < shortest.Length) {
                            shortest = temp;
                        }
                        
                        visited.Remove(src);
                    }
                }
            }

            return shortest;
        }
    }
}