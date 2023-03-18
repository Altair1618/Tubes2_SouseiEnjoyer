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
    }
}