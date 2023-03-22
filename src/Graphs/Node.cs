using System.Collections.Generic;

namespace Graphs {
    public class Node {
        // Attributes
        private static int nodeCount = 0;

        public int Id {
            get; private set;
        }
        
        /*
         * Node Categories:
         * N : Normal
         * T : Treasure
         * S : Start
         */
        public char Category {
            get; private set;
        }

        public List<Edge> Edges {
            get; private set;
        }

        // Constructor
        public Node(char category) {
            Id = nodeCount + 1;
            Category = category;
            Edges = new List<Edge>();
            nodeCount++;
        }

        // Methods
        // Add New Neighbor
        public void AddEdge(Node dest, string path) {
            Edges.Add(new Edge(this, dest, path));
        }
    }

    public class Edge {
        // Attributes
        public Node Src {
            get; private set;
        }

        public Node Dest {
            get; private set;
        }

        public string Path {
            get; private set;
        }

        public int Weight {
            get { return Path.Length; }
        }

        // Constructor
        public Edge(Node src, Node dest, string path) {
            Src = src;
            Dest = dest;
            Path = path;
        }
    }
}