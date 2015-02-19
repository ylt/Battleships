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
				List<Position> shipPath = shipinst.GetPath ();
				if (shipPath.Contains (pos))
					return this;
			}
			return null;
		}

		public ShotType FireAtShip(Position pos)
		{
			ShipInstance hit = HitsShip(pos);
			if (hit != null)
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

