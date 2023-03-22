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
        private int tileSize;
        public TileView(Tile tile, int tileSize = 100)
        {
            InitializeComponent();

            // tileBorder.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
            this.tileSize = tileSize;
            if (tile.IsWalkable() || tile.IsStartingPoint())
            {
                tileBorder.Background = new SolidColorBrush(Colors.LightSteelBlue);
            }
            else
            {
                tileBorder.Background = new SolidColorBrush(Colors.Black);
            }

            if (tile.IsTreasure() || tile.IsStartingPoint())
            {
                // Create Image Element
                /*                Border b2 = new Border();
                                ImageBrush image = new ImageBrush();

                                BitmapImage myBitmapImage = new BitmapImage();

                                myBitmapImage.BeginInit();*/
                string uri = "";
                if (tile.IsTreasure())
                {
                    uri = "../../src/assets/treasure.png";
                } else if (tile.IsStartingPoint())
                {
                    uri = "../../src/assets/start.png";
                }
                this.addImage(uri);
            }


        }
        public void addImage(string uriPath)
        {
            tileBorder.Child = null;
            Border b2 = new Border();
            ImageBrush image = new ImageBrush();

            BitmapImage myBitmapImage = new BitmapImage();

            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(uriPath, UriKind.Relative);

            myBitmapImage.DecodePixelWidth = (int)(0.4 * tileSize);
            myBitmapImage.EndInit();

            image.ImageSource = myBitmapImage;
            b2.Background = image;
            b2.Width = (int)(0.4 * tileSize);
            b2.Height = (int)(0.4 * tileSize);
            tileBorder.Child = b2;
        }

        public void addImageOnImage(string uriPath)
        {
            if (tileBorder.Child != null)
            {
                Border b2 = new Border();
                ImageBrush image = new ImageBrush();

                BitmapImage myBitmapImage = new BitmapImage();

                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(uriPath, UriKind.Relative);

                myBitmapImage.DecodePixelWidth = (int)(0.4 * tileSize);
                myBitmapImage.EndInit();

                image.ImageSource = myBitmapImage;
                b2.Background = image;
                b2.Name = "imageOnImage";
                b2.Width = (int)(0.4 * tileSize);
                b2.Height = (int)(0.4 * tileSize);
                Border? tileBorderChild = tileBorder.Child as Border;
                if (tileBorderChild != null) tileBorderChild.Child = b2;
            } else
            {
                this.addImage(uriPath);
                Border tileBorderChild = null!;
                tileBorderChild = tileBorder.Child as Border;
                tileBorderChild.Name = "imageOnImage";
            }
        }
    } 
}