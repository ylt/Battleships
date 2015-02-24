using System;
using System.Collections.Generic;

namespace BattleShips
{
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

		public List<Position> GetPath()
		{
			List<Position> path = new List<Position>();

			Position shippos = pos;
			Position rotpos = new Position (rotation);

			for (int i = 0; i < ship.length; i++)
			{
				path.Add (shippos);

				shippos += rotpos;
			}

			return path;
		}

		public bool CheckOverlap(ShipInstance other)
		{
			List<Position> path = GetPath ();
			List<Position> otherpath = other.GetPath ();

			foreach (Position pos in path) {
				if (otherpath.Contains(pos))
				{
					return true;
				}
			}
			return false;
		}
	}
}

