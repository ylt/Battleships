using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using BattleShips.imp;

namespace BattleShips.imp
{
    [ServiceContract]
    interface IShipsService
    {
        [OperationContract]
        int NewPlayer();

        [OperationContract]
        void SetName(int playerId, string name);

        [OperationContract]
        List<ChatMessage> RetrieveMessages(int sequenceId);

        [OperationContract]
        void SendMessage(ChatMessage message);

        [OperationContract]
        bool AddShip(int playerId, ShipInstance ship);

        [OperationContract]
        ShotType Fire(int playerId, Position pos);

        [OperationContract]
        ShotType[] GetBoard(int playerId);

        [OperationContract]
        List<Ship> GetShips();
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    class ShipsService : IShipsService
    {
        private GameBase gb;
        private int players;

        public ShipsService()
        {
            gb = new GameBase();
            players = 0;
        }


        public int NewPlayer()
        {
            int playerId = players;
            players ++;

            gb.games[playerId].Name = "Player " + playerId.ToString();

            return playerId;
        }

        public void SetName(int playerId, string name)
        {
            gb.SendMessage(new ChatMessage("Info", String.Format("{0} is now known as {1}", gb.games[playerId].Name, name)));
            gb.games[playerId].Name = name;
        }

        public bool setReady(int playerId)
        {
            gb.games[playerId].Ready = true;

            if (gb.games[1 - playerId].Ready == true)
            {
                gb.StartGame();
                return true;
            }
            return false;
        }

        public List<ChatMessage> RetrieveMessages(int sequenceId)
        {
            return gb.RetrieveMessages(sequenceId);
        }

        public void SendMessage(ChatMessage message)
        {
            gb.SendMessage(message);
        }

        public bool AddShip(int playerId, ShipInstance ship)
        {
            //clients only have access to the ship ID field
            // read this out and inject in the real ship instance.
            ship.ship = gb.ships[ship.shipId];
            bool result = gb.games[playerId].AddShip(ship);

            //automate ready
            if (gb.games[playerId].ships.Count >= gb.ships.Count) //if pships > gbships SHOULD never happem, but.. you never know
            {
                setReady(playerId);
            }
            return result;
        }

        public ShotType Fire(int playerId, Position pos)
        {
            gb.SendMessage(new ChatMessage("SYSTEM", "turn"));
            ShotType stype = gb.games[1 - playerId].FireAtShip(pos);
            return stype;
        }

        public ShotType[] GetBoard(int playerId)
        {
            return gb.games[playerId].shots;
        }

        public List<Ship> GetShips()
        {
            return gb.ships;
        }
    }
}
