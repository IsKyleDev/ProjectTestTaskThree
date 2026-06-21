using System;
using System.Collections.Generic;
using TestTask.NonEditable;
using UnityEngine;

namespace TestTask.Editable
{
    public static class ClientPacketsHandler
    {
        #region Packet Handlers
        public static void LoginDataReceived(Packet packet)
        {
            int responseCode = packet.ReadInt();
            int clientId = packet.ReadInt();

            ClientManager.Instance.SetClientLogInStatus(responseCode, clientId);
        }

        public static void MonsterDataRecieved(Packet packet)
        {
            int monsterId = packet.ReadInt();

            MonsterNames monsterType =
                (MonsterNames)packet.ReadInt();

            float maxHealth = packet.ReadFloat();

            float currentHealth = packet.ReadFloat();

            MonsterData monster = new MonsterData(monsterId, monsterType, maxHealth, currentHealth);

            ClientManager.Instance.ClientMobsManager.SetMonster(monster);
        }
        #endregion

        #region Packet Senders
        public static void SendLoginRequest()
        {
            Packet packet = new Packet(1);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }
        #endregion
    }
}
