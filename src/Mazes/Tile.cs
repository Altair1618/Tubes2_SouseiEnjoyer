using System.IO;

namespace Mazes {
    public class Tile {
        // Attributes
        private static int tileCount = 0;

        public int Id {
            get; private set;
        }

        public char Category {
            get; private set;
        }

        public int Visited
        {
            get; set;
        }

        // Constructor
        public Tile() {
            this.Id = tileCount + 1;
            this.Category = 'U';
            this.Visited = 0;
            tileCount++;
        }

        public Tile(char Category) {
            this.Id = tileCount + 1;
            this.Category = Category;
            if (Category != 'K' || Category != 'X' || Category != 'T' || Category != 'R')
            {
                throw new FileFormatException();
            }
            this.Visited = 0;
            tileCount++;
        }

        public bool IsStartingPoint() {
            return Category == 'K';
        }

        public bool IsTreasure() {
            return Category == 'T';
        }

        public bool IsWalkable() {
            return (Category == 'R' || Category == 'T');
        }

        public bool isWalkable() {
            return (Category == 'R' || Category == 'T' || Category == 'K');
        }
    }
}