using System;
using System.Collections.Generic;

namespace BattleShips
{
	public class Board
	{


		public ShotType[,] shots = new ShotType[10,10];
		public List<ShipInstance> ships = new List<ShipInstance>();
		GameBase game;

		public Board(GameBase game)
		{
			this.game = game;
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

		public bool TestShipPosition(ShipInstance ship)
		{
			foreach (ShipInstance shipinst in ships) {
				if (!shipinst.CheckOverlap (ship))
					return false;
			}
			return true;
		}

		public bool TestShipDuplicate(Ship ship)
		{
			List<ShipInstance> results = from shipinst in ships where shipinst.ship == ship select shipinst;
			if (results.Count > 0)
				return true;
			else
				return false;
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

