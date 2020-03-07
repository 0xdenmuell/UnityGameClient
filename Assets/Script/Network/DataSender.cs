namespace Assets.Script
{
    /*
     * All available packages that the client can send to the server
     */
    public enum ClientPackets
    {
        CWelcomeMessage = 1,
        CTeamSelectedRed = 10,
        CTeamSelectedBlue = 20,
        CLocationClient = 2,
        CDisconnectClient = 3,
        CRocketHasBeenSpawned = 4,
        CPlayerDied = 5,
        COtherPlayerDamage = 6
    }

    /*
     * If the client wants to send a message to the server
     */

    // TO DO: Dont use a function for each packet. Get a Switch Case

    class DataSender
    {
        //Send after the first Welcome Message a Message, which confirms the Connection
        public static void SendConnected()
        {
            string welcomeMessage = StartMenuUI.instanceUI.usernameField.text + " is connected to the Server.";
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CWelcomeMessage);
            buffer.WriteString(welcomeMessage);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();

        }

        // Team Red = 1
        // Team Blue = 2
        // TO DO: Where the Client has to be spawned should be calculated in the NetworkManager
        public static void SendTeamSelected(int team)
        {
            if (team == 1)
            {
                string teamMessage = StartMenuUI.instanceUI.usernameField.text + " is connected to Team " + team;
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ClientPackets.CTeamSelectedRed);
                buffer.WriteString(teamMessage);
                ClientTCP.SendData(buffer.ToArray());
                buffer.Dispose();

                StartMenuUI.instanceUI.TeamsitesUI.SetActive(true);

            }
            if (team == 2)
            {
                string teamMessage = StartMenuUI.instanceUI.usernameField.text + " is connected to Team " + team;
                ByteBuffer buffer = new ByteBuffer();
                buffer.WriteInteger((int)ClientPackets.CTeamSelectedBlue);
                buffer.WriteString(teamMessage);
                ClientTCP.SendData(buffer.ToArray());
                buffer.Dispose();

                StartMenuUI.instanceUI.UsernameUI.SetActive(false);
                StartMenuUI.instanceUI.TeamsitesUI.SetActive(true);
            }
        }

        //The Client sends his Positon and Rotation to the Server, so the Server syncs the Data with every other Client on the Match
        public static void SendPositionClientToServer(string serverMessage)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CLocationClient);
            buffer.WriteString(serverMessage);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

        //If the Local Client (myself) wants to quit the Game
        public static void SendDisconnetOfLocalClient(string serverMessage)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CDisconnectClient);
            buffer.WriteString(serverMessage);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public static void CRocketHasBeenSpawned(string serverMessage)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CRocketHasBeenSpawned);
            buffer.WriteString(serverMessage);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public static void CLocalPlayerDied(string serverMessage)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CPlayerDied);
            buffer.WriteString(serverMessage);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public static void COtherPlayerHasTakenDamage(string serverMessage)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.COtherPlayerDamage);
            buffer.WriteString(serverMessage);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

    }

}

