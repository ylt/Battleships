using System;
using System.Runtime.Serialization;

namespace BattleShips.imp
{
	[DataContract]
	public class Ship
	{
		[DataMember] string name;
		[DataMember] public int length;

		public Ship(string name, int length)
		{
			this.name = name;
			this.length = length;
		}
	}


}

