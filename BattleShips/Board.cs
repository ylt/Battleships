using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips
{
    enum shot_type
    {
        UNFIRED,
        MISS,
        HIT
    }

    enum rotation
    {
        RIGHT,
        DOWN,
        LEFT,
        UP
    }

    class Position
    {
        public int x;
        public int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Ship
    {
        string name;

        public int length;

        public Ship(string name, int length)
        {
            this.name = name;
            this.length = length;
        }
    }

    class ShipInstance
    {
        public Position pos;
        public rotation rotation;

        public bool sunken;

        public Ship ship;
        public ShipInstance(Position pos, rotation r, Ship ship)
        {
            this.pos = pos;
            this.rotation = r;
            this.ship = ship;

            this.sunken = false;
        }
    }

    class GameBase
    {
    }

    class Lobby
    {
        public Board[] games;
        public Game game;
        public Lobby()
        {
            games = new Board[2]{
                new Board(this),
                new Board(this)
            };
        }
    }

    class Game
    {
        public Lobby lobby;
        public Board[] games;
        public Dictionary<rotation, Position> rotations;
        public List<Ship> ships;

        public Game(Lobby lobby)
        {
            this.lobby = lobby;
            this.games = (Board[])lobby.games.Clone(); //clone for security reasons

            //create the rotation dictionary (allows converting into vector)
            rotations = new Dictionary<rotation, Position>();
            rotations.Add(rotation.RIGHT, new Position( 1,  0));
            rotations.Add(rotation.DOWN,  new Position( 0,  1));
            rotations.Add(rotation.LEFT,  new Position(-1,  0));
            rotations.Add(rotation.UP,    new Position( 0, -1));

            //create ships list
            ships = new List<Ship>();
            ships.Add(new Ship("Aircraft carrier", 5));
            ships.Add(new Ship("BattleShip", 4));
            ships.Add(new Ship("Cruiser", 3));
            ships.Add(new Ship("Cruiser", 3));
            ships.Add(new Ship("Destroyer", 2));
        }

    }

    class Board
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

        public bool HitsShip(Position pos)
        {
            foreach(ShipInstance shipinst in ships)
            {
                for (int i = 0; i < shipinst.ship.length; i++)
                {
                    int cx = shipinst.pos.x + game.rotations[shipinst.rotation].x;
                    int cy = shipinst.pos.y + game.rotations[shipinst.rotation].y;

                    if (pos.x == cx && pos.y == cy)
                        return true;
                }
            }
            return false;
        }

        public shot_type FireAtShip(Position pos)
        {
            bool hit = HitsShip(pos);
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

