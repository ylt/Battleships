using System;

namespace BattleShips
{
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
			case Rotation.LEFT:
				this.x = -1;
				break;
			case Rotation.UP:
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

}

