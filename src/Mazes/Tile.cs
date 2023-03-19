using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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

<<<<<<< HEAD
        public bool IsStartingPoint() {
            return Category == 'K';
        }

        public bool IsTreasure() {
            return Category == 'T';
        }

        public bool IsWalkable() {
            return (Category == 'K' || Category == 'R' || Category == 'T');
        }

        public char GetCategory()
        {
            return Category;
        }
=======
>>>>>>> 175eb57 (add: maze and tile view)
    }
}