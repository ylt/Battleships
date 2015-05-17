using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace BattleShips.imp
{
	public class Board
	{
        const int boardSize = 10;
		public ShotType[] shots = new ShotType[boardSize*boardSize];
		public List<ShipInstance> ships = new List<ShipInstance>();
		GameBase game;

		public Board(GameBase game)
		{
			this.game = game;
		}

        public bool AddShip(ShipInstance ship)
        {
            if (TestShipDuplicate(ship.ship)) {
                throw new FaultException("Duplicate Ship (cheater)");
            }
            else if (ship.pos.x >= 0 && ship.pos.x < boardSize &&
                (ship.rotation != Rotation.RIGHT || ship.pos.x + ship.ship.length < boardSize) && //EITHER ship is not pointing right, or if it is pointing right then is within grid
                ship.pos.y >= 0 && ship.pos.y < boardSize &&
                (ship.rotation != Rotation.DOWN || ship.pos.y + ship.ship.length < boardSize))
            {
                if (TestShipPosition(ship))
                {
                    ships.Add(ship);
                    return true;
                }
                else
                {
                    throw new FaultException("Overlapping with another ship");
                }
            }
            else
            {
                throw new FaultException("Going outside grid");
            }
        }

		public ShipInstance HitsShip(Position pos)
		{
			foreach(ShipInstance shipinst in ships)
			{
				List<Position> shipPath = shipinst.GetPath ();
				if (shipPath.Contains (pos))
					return shipinst;
			}
			return null;
		}

		public bool TestShipPosition(ShipInstance ship)
		{
			foreach (ShipInstance shipinst in ships) {
				if (shipinst.CheckOverlap (ship))
					return false;
			}
			return true;
		}

		public bool TestShipDuplicate(Ship ship)
		{
			//var results = from shipinst in ships where shipinst.ship == ship select shipinst;
            
            List<ShipInstance> results = ships.Where(x => x.ship == ship).ToList();
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
                shots[pos.x + (pos.y * boardSize)] = ShotType.HIT;
			}
			else
			{
                shots[pos.x + (pos.y * boardSize)] = ShotType.MISS;
			}
			return shots[pos.x + (pos.y * boardSize)];
		}
	}
}

