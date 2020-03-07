using Assets.Script;
using System.Collections.Generic;

namespace UnityGameServer
{
    /*
     * This class initializes packets that are sent between server and client. 
     * In simple language: "If i get Packet 4 from the Server, i have to go to Function 4".
     * It also checks if the received bits are correct.
     */

    static class ClientHandleData
    {
        private static ByteBuffer playerBuffer;
        public delegate void Packet(byte[] data);
        public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

        //These Packets are getting send by the Server
        //Validate which action should happen now
        public static void InitializePackets()
        {
            //This Packet will be received after the Client is connected to the Server
            packets.Add((int)ServerPackets.SWelcomeMessage, DataReceiver.HandleWelcomeMessage);

            //After the Server has choosen where the Player spawns, this packets will be received
            packets.Add((int)ServerPackets.SSpawnPlayerRed11, DataReceiver.HandleSpawnPlayer);
            packets.Add((int)ServerPackets.SSpawnPlayerRed12, DataReceiver.HandleSpawnPlayer);
            packets.Add((int)ServerPackets.SSpawnPlayerRed13, DataReceiver.HandleSpawnPlayer);
            packets.Add((int)ServerPackets.SSpawnPlayerBlue21, DataReceiver.HandleSpawnPlayer);
            packets.Add((int)ServerPackets.SSpawnPlayerBlue22, DataReceiver.HandleSpawnPlayer);
            packets.Add((int)ServerPackets.SSpawnPlayerBlue23, DataReceiver.HandleSpawnPlayer);

            //After the Client has been spawned on the Map, the Location of every other Client will be transmitted
            packets.Add((int)ServerPackets.SLocationClient, DataReceiver.HandleLocationPlayer);

            //If a Client has disconnected from the Server, this packet will be received
            packets.Add((int)ServerPackets.SDisconnectClient, DataReceiver.HandleDisconnectClient);

            packets.Add((int)ServerPackets.SRocketHasBeenSpawned, DataReceiver.HandleRocketHasBeenSpawned);

            packets.Add((int)ServerPackets.SPlayerDied, DataReceiver.HandlePlayerDied);

            packets.Add((int)ServerPackets.HandlePlayerHasTakenDamage, DataReceiver.HandlePlayerHasTakenDamage);
        }

        //Checks if the Transmisson is correct transmitted
        public static void HandleData(byte[] data)
        {
            byte[] buffer = (byte[])data.Clone();
            int pLength = 0;

            if (playerBuffer == null)
            {
                playerBuffer = new ByteBuffer();
            }

            playerBuffer.WriteBytes(buffer);

            if (playerBuffer.Count() == 0)
            {
                playerBuffer.Clear();
                return;
            }

            if (playerBuffer.Length() >= 4)
            {
                pLength = playerBuffer.ReadInteger(false);
                if (pLength <= 0)
                {
                    playerBuffer.Clear();
                    return;
                }
            }

            while (pLength > 0 & pLength <= playerBuffer.Length() - 4)
            {
                if (pLength <= playerBuffer.Length() - 4)
                {
                    playerBuffer.ReadInteger();
                    data = playerBuffer.ReadBytes(pLength);
                    HandleDataPackets(data);
                }

                pLength = 0;
                if (playerBuffer.Length() >= 4)
                {
                    pLength = playerBuffer.ReadInteger(false);
                    if (pLength <= 0)
                    {
                        playerBuffer.Clear();
                        return;
                    }
                }
            }
            if (pLength <= 1)
            {
                playerBuffer.Clear();
            }

        }

        //Validate which Serverpacket has been sent
        private static void HandleDataPackets(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            buffer.Dispose();
            if (packets.TryGetValue(packetID, out Packet packet))
            {
                packet.Invoke(data);
            }
        }
    }
}