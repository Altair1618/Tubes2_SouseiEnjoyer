using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mazes;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using IO;

namespace src
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void openFileButton(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                filename.Text = openFileDialog.SafeFileName;
                FileReader fileReader = new FileReader();
                Maze maze = fileReader.read(openFileDialog.FileName);
                foreach (var row in maze.maze)
                {
                    foreach(var col in row)
                    {
                        Trace.Write(col.Category + " ");
                    }
                    Trace.WriteLine("");
                }
            }
        }


    }
}
