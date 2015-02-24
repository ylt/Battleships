using System;

namespace BattleShips
{
	public class Game
	{
		public GameBase gamebase;

		public Board[] games;

		public Game(GameBase gamebase)
		{
			this.gamebase = gamebase;
			//this.games = (Board[])lobby.games.Clone(); //clone for security reasons
			//create ships list
		}

	}
}

