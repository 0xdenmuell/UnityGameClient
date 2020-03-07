using UnityEngine;

namespace Assets.Script
{

    /*
     * All available packages that the client can take from the server
     */

    public enum ServerPackets
    {
        SWelcomeMessage = 1,
        SSpawnPlayerRed11 = 11,
        SSpawnPlayerRed12 = 12,
        SSpawnPlayerRed13 = 13,
        SSpawnPlayerBlue21 = 21,
        SSpawnPlayerBlue22 = 22,
        SSpawnPlayerBlue23 = 23,
        SLocationClient = 2,
        SDisconnectClient = 3,
        SRocketHasBeenSpawned = 4,
        SPlayerDied = 5,
        HandlePlayerHasTakenDamage = 6
    }

    /*
     * This class receives the packet from the server and usually forwards it to the NetworkManager,
     * where the packet is processed.
     */

    class DataReceiver
    {
        //If the Server has established a connection to the Server, the Servers sends a Welcome Message
        public static void HandleWelcomeMessage(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            Debug.Log("Server: " + msg);
            StartMenuUI.instanceUI.UsernameUI.SetActive(false);
            StartMenuUI.instanceUI.TeamsitesUI.SetActive(true);
            DataSender.SendConnected();
        }

        //If a Client spawns new to a game (Username + Team + Spawnpoint)
        public static void HandleSpawnPlayer(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            Debug.Log("Server: " + msg);

            if (packetID < 13)
            {
                NetworkManager.instance.InstantiatePlayerRed(packetID, msg);
            }
            else
            {
                NetworkManager.instance.InstantiatePlayerBlue(packetID, msg);
            }
        }

        //Takes up the Location of every Client
        public static void HandleLocationPlayer(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string clientMsg = buffer.ReadString();
            buffer.Dispose();

            NetworkManager.instance.MoveAllClient(clientMsg);
        }

        //If a Client has disconnected and has to be destroyed
        public static void HandleDisconnectClient(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string clientMsg = buffer.ReadString();
            buffer.Dispose();

            NetworkManager.instance.DestroyClient(clientMsg);
        }
        public static void HandleRocketHasBeenSpawned(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string clientMsg = buffer.ReadString();
            buffer.Dispose();

            NetworkManager.instance.GetRocketInfos(clientMsg);
        }

        public static void HandlePlayerDied(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string clientMsg = buffer.ReadString();
            buffer.Dispose();

            NetworkManager.instance.DestroyClient(clientMsg);
        }

        public static void HandlePlayerHasTakenDamage(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string clientMsg = buffer.ReadString();
            buffer.Dispose();

            NetworkManager.instance.GivePlayerDamage(clientMsg);
        }

    }

}

