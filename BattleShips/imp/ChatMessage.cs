using System;
using System.Runtime.Serialization;

namespace BattleShips.imp
{
	[DataContract]
	public class ChatMessage
	{
		[DataMember] public int sequenceId;
		[DataMember] public DateTime Timestamp;
		[DataMember] public String user;
		[DataMember] public String message;

	    public ChatMessage(String user, String message)
	    {
            this.user = user;
            this.message = message;
	    }
	}
}

