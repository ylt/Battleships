using System;
using System.Runtime.Serialization;

namespace BattleShips
{
	[DataContract]
	public class ChatMessage
	{
		[DataMember] public int sequenceId;
		[DataMember] public DateTime Timestamp;
		[DataMember] public String user;
		[DataMember] public String message;
	}
}

