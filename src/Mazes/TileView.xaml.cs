using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
namespace Mazes
{
    public partial class TileView : Border
    {
        public TileView(Tile tile)
        {
            InitializeComponent();

            tileBorder.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
<<<<<<< HEAD
            if (!tile.IsWalkable())
=======
            if (tile.Category == 'X')
>>>>>>> 175eb57 (add: maze and tile view)
            {
                tileBorder.Background = new SolidColorBrush(Colors.Black);
            }
            else
            {
                tileBorder.Background = new SolidColorBrush(Colors.White);
            }

<<<<<<< HEAD
            if (tile.IsTreasure())
=======
            if (tile.Category == 'T')
>>>>>>> 175eb57 (add: maze and tile view)
            {
                // Create Image Element
                Border b2 = new Border();
                ImageBrush image = new ImageBrush();

                BitmapImage myBitmapImage = new BitmapImage();

                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri("../../src/assets/treasure.png", UriKind.Relative);

                myBitmapImage.DecodePixelWidth = 40;
                myBitmapImage.EndInit();

                image.ImageSource = myBitmapImage;
                b2.Background = image;
                b2.Width = 40;
                b2.Height = 40;
                tileBorder.Child = b2;
            }
        }
    }
}