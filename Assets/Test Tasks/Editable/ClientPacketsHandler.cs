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

        public static void MonsterDataReceived(Packet packet)
        {
            int version = packet.ReadInt();

            if (ClientManager.Instance.ClientMobsManager.IsVersionOutdated(version))
            {
                Debug.Log("Ignoring monster version: " + version);
                return;
            }

            int monsterId = packet.ReadInt();
            MonsterNames monsterType = (MonsterNames)packet.ReadInt();
            float maxHealth = packet.ReadFloat();
            float currentHealth = packet.ReadFloat();

            MonsterData monster = new MonsterData(monsterId, monsterType, maxHealth, currentHealth);
            ClientManager.Instance.ClientMobsManager.SetMonster(monster);
            ClientManager.Instance.ClientMobsManager.SetVersion(version);
        }
        #endregion

        #region Packet Senders
        public static void SendLoginRequest()
        {
            Packet packet = new Packet(1);
            ClientManager.Instance.PacketSenderClient.SendToServer(packet);
        }

        public static void SendMonsterTakeDamageRequest()
        {
            using (Packet packet = new Packet(2))
            {
                packet.Write(ClientManager.Instance.ClientMobsManager.MonsterData.MonsterId);

                ClientManager.Instance.PacketSenderClient.SendToServer(packet);
            }
        }
        #endregion
    }
}
