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
            Border imageBorder = new Border();
            ImageBrush image = new ImageBrush();

            BitmapImage bitmapImage = new BitmapImage();

            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(uriPath, UriKind.Relative);

            bitmapImage.DecodePixelWidth = (int)(0.4 * tileSize);
            bitmapImage.EndInit();

            image.ImageSource = bitmapImage;
            imageBorder.Background = image;
            imageBorder.Width = (int)(0.4 * tileSize);
            imageBorder.Height = (int)(0.4 * tileSize);
            tileBorder.Child = imageBorder;
        }

        public void addImageOnImage(string uriPath)
        {
            if (tileBorder.Child != null)
            {
                Border imageBorder = new Border();
                ImageBrush image = new ImageBrush();

                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(uriPath, UriKind.Relative);

                bitmapImage.DecodePixelWidth = (int)(0.4 * tileSize);
                bitmapImage.EndInit();

                image.ImageSource = bitmapImage;
                imageBorder.Background = image;
                imageBorder.Name = "imageOnImage";
                imageBorder.Width = (int)(0.4 * tileSize);
                imageBorder.Height = (int)(0.4 * tileSize);
                Border? tileBorderChild = tileBorder.Child as Border;
                if (tileBorderChild != null) tileBorderChild.Child = imageBorder;
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