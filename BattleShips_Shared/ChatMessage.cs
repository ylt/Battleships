using System;
using System.Runtime.Serialization;

namespace BattleShips
{
	[DataContract]
	public class ChatMessage
	{
		public int sequenceId;
		[DataMember]
		public DateTime Timestamp;
		[DataMember]
		public String user;
		[DataMember]
		public String message;
	}
}

