using System.Collections.Generic;

namespace Mazes {
    public class Maze {
        // Attributes
        public List <List <Tile>> maze {
            get; private set;
        }

        // Constructor
        public Maze() {
            maze = new List<List<Tile>>();
        }
    }
}