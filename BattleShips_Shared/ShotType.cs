using System;
using System.Runtime.Serialization;

namespace BattleShips
{
	[DataContract]
	public enum ShotType
	{
		[EnumMember] UNFIRED,
		[EnumMember] MISS,
		[EnumMember] HIT
	}
}

