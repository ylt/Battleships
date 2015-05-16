﻿using System;
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
        List<ChatMessage> RetrieveMessages(int sequenceId);

        [OperationContract]
        void SendMessage(ChatMessage message);

        [OperationContract]
        bool AddShip(int playerId, ShipInstance ship);

        [OperationContract]
        ShotType Fire(int playerId, Position pos);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    class ShipsService : IShipsService
    {
        private GameBase gb;

        public ShipsService()
        {
            gb = new GameBase();
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
            return gb.games[playerId].AddShip(ship);
        }

        public ShotType Fire(int playerId, Position pos)
        {
            gb.SendMessage(new ChatMessage("SYSTEM", "turn"));
            ShotType stype = gb.games[1 - playerId].FireAtShip(pos);
            return stype;
        }
    }
}