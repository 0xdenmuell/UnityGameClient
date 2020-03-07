using System;
using System.Linq;
using System.Net.Sockets;
using UnityGameServer;

namespace Assets.Script
{
    /*
     * This class establishes a connection between the server and client.
     * After a connection is established, a stream is created to send bits back and forth between the two partners.
     */

    static class ClientTCP
    {
        private static TcpClient clientSocket;
        private static NetworkStream myStream;
        private static byte[] recBuffer;


        //Try connecting to the Server
        public static void InitializingNetworking()
        {
            clientSocket = new TcpClient();
            clientSocket.ReceiveBufferSize = 4096;
            clientSocket.SendBufferSize = 4096;
            recBuffer = new byte[4096 * 2];
            clientSocket.BeginConnect("127.0.0.1", 5557, new AsyncCallback(ClientConnectCallback), clientSocket);
        }


        //Callback after connecting with the Server
        //Begins reading the Server
        private static void ClientConnectCallback(IAsyncResult result)
        {
            try
            {
                clientSocket.EndConnect(result);
            }
            catch (Exception noConnection)
            {
                string usernameKey = NetworkManager.playerList.FirstOrDefault(x => x.Value.isLocalPlayer == true).Key;
                NetworkManager.playerList.Remove(usernameKey);
                UnityThread.executeInFixedUpdate(() =>
                {
                    StartMenuUI.instanceUI.NoConnection();
                });
                return;
            }

            clientSocket.NoDelay = true;
            myStream = clientSocket.GetStream();
            myStream.BeginRead(recBuffer, 0, 4096 * 2, ReceiveCallback, null);

        }

        //If the Welcome Message arrives
        //it begins to endless loop all other Messages in different Unity Thread
        private static void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int length = myStream.EndRead(result);
                if (length <= 0)
                {
                    return;
                }


                byte[] newBytes = new byte[length];
                Array.Copy(recBuffer, newBytes, length);
                UnityThread.executeInFixedUpdate(() =>
                {
                    ClientHandleData.HandleData(newBytes);
                });
                //Infinte Loop
                myStream.BeginRead(recBuffer, 0, 4096 * 2, ReceiveCallback, null);
            }

            catch (Exception)
            {

                //NetworkManager.instance.DestroyLocalPlayer();
                return;
            }
        }

        //If the Client wants to send a Packet to the Server
        public static void SendData(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.WriteBytes(data);
            myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
            buffer.Dispose();
        }
    }
}
