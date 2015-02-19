using System;
using System.Collections.Generic;

namespace BattleShips
{
	public class GameBase
	{
		public Board[] games;
		public List<Ship> ships;
		
		public Game game;

		public List<ChatMessage> messages;
		int messageId = 0;

		private bool gameStarted = false;

		public GameBase()
		{
			ships = new List<Ship>();
			ships.Add(new Ship("Aircraft carrier", 5));
			ships.Add(new Ship("BattleShip", 4));
			ships.Add(new Ship("Cruiser1", 3));
			ships.Add(new Ship("Cruiser2", 3));
			ships.Add(new Ship("Destroyer", 2));

			games = new Board[2] {
				new Board(this),
				new Board(this)
			};

			messages = new List<ChatMessage> ();
		}

		public void StartGame()
		{
			this.game = new Game(this);
			gameStarted = true;
		}

		public void SendMessage(ChatMessage message)
		{
			message.sequenceId = messageId;
			messages.Add (message);
		}

		public List<ChatMessage> RetrieveMessages(int messageId)
		{
			List<ChatMessage> newmsg = from message in messages where message.sequenceId > messageId select message;
			return newmsg;
		}
	}
}

