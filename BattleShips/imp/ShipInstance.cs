using System;

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
	}
}

