namespace Mazes {
    public class Tile {
        // Attributes
        private static int tileCount = 0;

        public int Id {
            get; private set;
        }

        private char Category;

        // Constructor
        public Tile() {
            this.Id = tileCount + 1;
            this.Category = 'U';
            tileCount++;
        }

        public Tile(char Category) {
            this.Id = tileCount + 1;
            this.Category = Category;
            tileCount++;
        }

        public bool IsStartingPoint() {
            return Category == 'K';
        }

        public bool IsTreasure() {
            return Category == 'T';
        }

        public bool IsWalkable() {
            return (Category == 'K' || Category == 'R' || Category == 'T');
        }
    }
}