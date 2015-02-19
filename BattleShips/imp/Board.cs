using System;
using System.Collections.Generic;

namespace BattleShips
{
	public class Board
	{


		public ShotType[,] shots = new ShotType[10,10];
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

		public ShotType FireAtShip(Position pos)
		{
			ShipInstance hit = HitsShip(pos);
			if (hit)
			{
				shots[pos.x, pos.y] = ShotType.HIT;
			}
			else
			{
				shots[pos.x, pos.y] = ShotType.MISS;
			}
			return shots[pos.x, pos.y];
		}
	}
}

