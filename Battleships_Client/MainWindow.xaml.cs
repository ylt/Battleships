using Battleships_Client.BShipService;
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

namespace Battleships_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();


            playerGrid.ShowGridLines = true;
            opponentGrid.ShowGridLines = true;

            for (int i = 0; i < 10+1; i++)
            {
                playerGrid.ColumnDefinitions.Add(new ColumnDefinition());
                playerGrid.RowDefinitions.Add(new RowDefinition());

                opponentGrid.ColumnDefinitions.Add(new ColumnDefinition());
                opponentGrid.RowDefinitions.Add(new RowDefinition());

                if (i != 0)
                {
                    createLabel(playerGrid, i);
                    createLabel(opponentGrid, i);
                }
            }

            buttoniseGrid(playerGrid);
        }

        private void createLabel(Grid grid, int i)
        {
            TextBlock xLabel = new TextBlock();
            xLabel.Text = new string((char)('A' + i - 1), 1);
            xLabel.FontWeight = FontWeights.Bold;
            xLabel.VerticalAlignment = VerticalAlignment.Center;
            xLabel.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(xLabel, 0);
            Grid.SetColumn(xLabel, i);
            grid.Children.Add(xLabel);

            TextBlock yLabel = new TextBlock();
            yLabel.Text = (i.ToString());
            yLabel.FontWeight = FontWeights.Bold;
            yLabel.VerticalAlignment = VerticalAlignment.Center;
            yLabel.HorizontalAlignment = HorizontalAlignment.Center;
            Grid.SetRow(yLabel, i);
            Grid.SetColumn(yLabel, 0);
            grid.Children.Add(yLabel);
        }

        private void clearGrid(Grid grid)
        {
            List<UIElement> remove = new List<UIElement>();

            foreach (UIElement child in grid.Children)
            {
                if (Grid.GetRow(child) != 0 || Grid.GetColumn(child) != 0)
                {
                    //grid.Children.Remove(child);
                    remove.Add(child);
                }
            }

            foreach(UIElement el in remove)
            {
                grid.Children.Remove(el);
            }
        }

        private GridEntry[,] buttoniseGrid(Grid grid)
        {
            GridEntry[,] buttons = new GridEntry[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    GridEntry button = new GridEntry();
                    Grid.SetColumn(button, x+1);
                    Grid.SetRow(button, y+1);
                    grid.Children.Add(button);
                }
            }
            return buttons;
        }
        
    }

    class GridEntry : Button
    {
        Ship ship;
        ShotType stype;

        public GridEntry() : base()
        {
            this.ShotType = ShotType.MISS;
        }

        public ShotType ShotType {
            get
            {
                return stype;
            }
            set
            {
                this.stype = value;

            }
        }

    }
}
