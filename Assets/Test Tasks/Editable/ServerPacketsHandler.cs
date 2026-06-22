using TestTask.NonEditable;
using UnityEngine;

namespace TestTask.Editable
{
    public static class ServerPacketsHandler
    {
        #region Packet Handlers
        public static void LoginRequest(Packet packet)
        {
            var clientLogInResponse = ServerMock.Instance.TryConnectClient(out var clientId);
            SendLoginResponse(clientLogInResponse, clientId);

            if (clientLogInResponse == LoginResponse.Success)
            {
                // Inform the client about this monster via a packet.
                SendMonsterData();
            }
        }

        public static void MonsterTakeDamageRequest(Packet packet)
        {
            int monsterId = packet.ReadInt();

            if (monsterId == ServerMock.Instance.ServerMobsManager.MonsterData.MonsterId)
            {
                ServerMock.Instance.ServerMobsManager.HandleMonsterTakeDamage();
            }
            else
            {
                Debug.Log("Monster ID does not match, returning.");
            }
 
        }


        #endregion

        #region Packet Senders
        public static void SendLoginResponse(LoginResponse response, int clientId)
        {
            using (Packet packet = new Packet(1))
            {
                packet.Write((int)response);
                packet.Write(clientId);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        public static void SendMonsterData()
        {

            MonsterData monster = ServerMock.Instance.ServerMobsManager.MonsterData;

            using (Packet packet = new Packet(2))
            {
                packet.Write(ServerMock.Instance.ServerMobsManager.MonsterStateVersion);
                packet.Write(monster.MonsterId);
                packet.Write((int)monster.MonsterType);
                packet.Write(monster.MonsterMaxHealth);
                packet.Write(monster.MonsterCurrentHealth);

                ServerMock.Instance.PacketSenderServer.SendToClient(packet);
            }
        }

        #endregion
    }
}

public enum LoginResponse
{
    Success = 0,
    Failure = 1,
}