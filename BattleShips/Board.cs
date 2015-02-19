using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips
{
    public enum shot_type
    {
        UNFIRED,
        MISS,
        HIT
    }

    public enum Rotation
    {
        RIGHT,
        DOWN,
        LEFT,
        UP
    }

    public class Position
    {
        public int x;
        public int y;

        public Position(Rotation rot)
        {
            x = 0;
            y = 0;
            switch (rot) {
            case Rotation.RIGHT:
                this.x = 1;
                break;
            case Rotation.DOWN:
                this.y = 1;
                break;
            case Rotation.RIGHT:
                this.x = -1;
                break;
            case Rotation.DOWN:
                this.y = -1;
                break;
            }
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        //https://msdn.microsoft.com/en-us/library/vstudio/336aedhh%28v=vs.100%29.aspx
        public override int GetHashCode() 
        {
            return x ^ y;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || !(obj is Position))
                return false;

            Position pos2 = (Position)obj;

            if (pos2.x == x && pos2.y == y)
                return true;
            else
                return false;
        }

        public static Position operator + (Position left, Position right)
        {
            return new Position (left.x + right.x, left.y + right.y);
        }

        public static Position operator - (Position left, Position right)
        {
            return new Position (left.x - right.x, left.y - right.y);
        }
    }

    public class Ship
    {
        string name;

        public int length;

        public Ship(string name, int length)
        {
            this.name = name;
            this.length = length;
        }
    }

    public class ShipInstance
    {
        public Position pos;
        public Rotation rotation;

        public bool sunken;

        public Ship ship;
        public ShipInstance(Position pos, Rotation r, Ship ship)
        {
            this.pos = pos;
            this.rotation = r;
            this.ship = ship;

            this.sunken = false;
        }
    }

	public class ChatMessage
	{
		int sequenceId;
		public DateTime Timestamp;
		public String user;
		public String message;
	}

    public class GameBase
    {
		public Board[] games;
		public List<Ship> ships;

		public Lobby lobby;
		public Game game;
		public List<ChatMessage> messages;

		private bool gameStarted = false;

		public GameBase()
		{
			ships = new List<Ship>();
			ships.Add(new Ship("Aircraft carrier", 5));
			ships.Add(new Ship("BattleShip", 4));
			ships.Add(new Ship("Cruiser1", 3));
			ships.Add(new Ship("Cruiser2", 3));
			ships.Add(new Ship("Destroyer", 2));

			games = new Board[2] {
				new Board(this),
				new Board(this)
			};

			messages = new List<ChatMessage> ();
		}

		public void StartGame()
		{
			this.game = new Game(this);
			gameStarted = true;
		}
    }

    public class Game
    {
		public GameBase gamebase;

		public Board[] games;

		public Game(GameBase gamebase)
        {
			this.gamebase = gamebase;
            //this.games = (Board[])lobby.games.Clone(); //clone for security reasons
            //create ships list

        }

    }

    public class Board
    {


        public shot_type[,] shots = new shot_type[10,10];
        public List<ShipInstance> ships = new List<ShipInstance>();
        Game game;

        public Board(Game game)
        {
            this.game = game;


            /*ships.Add(new ShipInstance(
                new Position(0,0),
                rotation.RIGHT,
                game.ships[1]
           ));*/
        }

        public ShipInstance HitsShip(Position pos)
        {
            foreach(ShipInstance shipinst in ships)
            {
                Position shippos = shipinst.pos;
                for (int i = 0; i < shipinst.ship.length; i++)
                {
                    if (pos.Equals(shippos))
                        return shipinst;

                    shippos += new Position (shipinst.rotation);
                }
            }
            return null;
        }

        public shot_type FireAtShip(Position pos)
        {
            ShipInstance hit = HitsShip(pos);
            if (hit)
            {
                shots[pos.x, pos.y] = shot_type.HIT;
            }
            else
            {
                shots[pos.x, pos.y] = shot_type.MISS;
            }
            return shots[pos.x, pos.y];
        }
    }
}

