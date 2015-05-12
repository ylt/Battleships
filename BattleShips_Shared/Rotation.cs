using System;
using System.Runtime.Serialization;

namespace BattleShips
{
	[DataContract]
	public enum Rotation
	{
		[EnumMember] RIGHT,
		[EnumMember] DOWN,
		[EnumMember] LEFT,
		[EnumMember] UP
	}
}

