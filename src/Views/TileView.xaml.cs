using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Mazes;
namespace Views
{
    public partial class TileView : Border
    {
        public TileView(Tile tile)
        {
            InitializeComponent();

            tileBorder.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
            if (tile.IsWalkable() || tile.IsStartingPoint()) 
            {
                tileBorder.Background = new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                tileBorder.Background = new SolidColorBrush(Colors.Black);
            }

            if (tile.IsTreasure() || tile.IsStartingPoint())
            {
                // Create Image Element
                Border b2 = new Border();
                ImageBrush image = new ImageBrush();

                BitmapImage myBitmapImage = new BitmapImage();

                myBitmapImage.BeginInit();
                string uri = "";
                if (tile.IsTreasure())
                {
                    uri = "../../src/assets/treasure.png";
                } else if (tile.IsStartingPoint())
                {
                    uri = "../../src/assets/start.png";
                }
                myBitmapImage.UriSource = new Uri(uri, UriKind.Relative);

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