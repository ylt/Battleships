using System;
using System.Runtime.Serialization;

namespace BattleShips.imp
{
	[DataContract]
	public enum ShotType
	{
		[EnumMember] UNFIRED,
		[EnumMember] MISS,
		[EnumMember] HIT
	}
}

