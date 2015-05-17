using Battleships_Client.BShipService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Battleships_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //gui vars
        private GridEntry[,] playerButtons;
        private GridEntry[,] opponentButtons;

        private DispatcherTimer timer;

        //gameplay vars
        private BShipService.ShipsServiceClient bships;
        int messageId = -1;

        private int playerId;
        private int gameState = 0;

        private Ship[] shipTypes;
        private BShipService.Rotation rotation;
        private int shipId;

        public MainWindow()
        {
            InitializeComponent();


            //initialise gui
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

            playerButtons = buttoniseGrid(playerGrid, GridButtonClick);
            opponentButtons = buttoniseGrid(opponentGrid, GridButtonClick);
            buttonState(opponentButtons, false);

            //initialise gameplay related stuff
            bships = new BShipService.ShipsServiceClient();
            playerId = bships.NewPlayer();
            shipTypes = bships.GetShips();

            PlaceShipMsg();


            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(CheckChat);
            timer.Interval = new TimeSpan(0, 0, 1); //hours, minutes, seconds, milliseconds
            timer.Start();
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

        private GridEntry[,] buttoniseGrid(Grid grid, RoutedEventHandler handler)
        {
            GridEntry[,] buttons = new GridEntry[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    GridEntry button = new GridEntry(x, y);

                    button.Click += handler;

                    Grid.SetColumn(button, x+1);
                    Grid.SetRow(button, y+1);
                    grid.Children.Add(button);

                    buttons[x, y] = button;
                }
            }
            return buttons;
        }

        private void buttonState(GridEntry[,] grid, bool state)
        {
            foreach (GridEntry entry in grid)
            {
                entry.IsEnabled = state;
            }
        }

        
        private void GridButtonClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is GridEntry))
            {
                return;
            }

            GridEntry entry = sender as GridEntry;
            switch(gameState) {
                case 0: //placement mode
                    PlaceShip(entry.X, entry.Y);
                    break;

                case 1: //playing mode
                    break;
            }
        }

        private void PlaceShipMsg()
        {
            Ship ship = shipTypes[shipId];
            String rot = (rotation == BShipService.Rotation.DOWN) ? "down" : "right";
            AddChatMessage("Help", String.Format("Please place the {0} (length: {1}), current rotation: {2}", ship.name, ship.length, rot));
        }

        private void ChangeRotation(object sender, RoutedEventArgs e)
        {
            string rot;
            if (rotation == BShipService.Rotation.DOWN)
            {
                rotation = BShipService.Rotation.RIGHT;
                rot = "right";
            }
            else
            {
                rotation = BShipService.Rotation.DOWN;
                rot = "down";
            }
            AddChatMessage("Help", String.Format("New rotation: {0}", rot));
        }

        private void PlaceShip(int x, int y)
        {
            ShipInstance shipinst = new ShipInstance {
                pos = new Position { x = x, y = y},
                rotation = rotation,
                shipId = shipId
            };
            try
            {
                bships.AddShip(playerId, shipinst);

                //if we're here, success :D
                ColourShip(playerButtons, shipinst);

                shipId++; //NEXT!
                if (shipId < shipTypes.Length)
                {
                    PlaceShipMsg();
                }
                else
                {
                    DonePlacing(); // :D
                }
            }
            catch (FaultException e)
            {
                AddChatMessage("Error", e.Reason.ToString());
            }
        }

        private void ColourShip(GridEntry[,] buttons, ShipInstance inst)
        {
            Position pos = inst.pos;
            for (int i = 0; i < shipTypes[inst.shipId].length; i++)
            {
                buttons[pos.x, pos.y].Background = Brushes.Yellow;

                //on the client side, the mathy helpers no-longer exist in Position :(
                if (inst.rotation == BShipService.Rotation.RIGHT)
                {
                    pos.x++;
                }
                else
                {
                    pos.y++;
                }

            }
        }

        private void DonePlacing()
        {
            AddChatMessage("Help", "You've placed your ships, game marked as ready.");
            buttonState(playerButtons, false);
        }

        private void ReceivedChatMessage(ChatMessage message)
        {
            messageId = message.sequenceId;
            AddChatMessage(message.user, message.message);
        }

        private void AddChatMessage(string name, string message)
        {
            //TODO: handle scroll
            chatBox.Items.Add(new ChatEntry { Name = name, Message = message });
        }

        private void CheckChat(object sender, EventArgs e)
        {
            //AddChatMessage("debug", "timer");
            ChatMessage[] messages = bships.RetrieveMessages(messageId);
            foreach(ChatMessage message in messages)
            {
                ReceivedChatMessage(message);
            }
        }
    }

    class GridEntry : Button
    {
        int x;
        int y;

        Ship ship;
        ShotType stype;

        public GridEntry(int x, int y) : base()
        {
            this.x = x;
            this.y = y;

            this.ShotType = ShotType.UNFIRED; //default to unfired as no shots have been made yet.
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

        public int X
        {
            get
            {
                return x;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
        }

    }

    class ChatEntry
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }

}
