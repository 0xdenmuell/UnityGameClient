using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityGameServer;
using Assets.Script.Network;
using System.Linq;

/*
 * This class should take over the main calculations of the client. 
 * Also the PlayerList is saved here
 */

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    //Player Prefab
    public GameObject playerPref;
    private bool alreadyTriedToConnect = false;

    //A list of Client is saved here
    public static Dictionary<string, Clients> playerList = new Dictionary<string, Clients>();

    private void Awake()
    {
        instance = this;
    }

    //This method gets called in the Beginning. It starts the UnityThread
    void Start()
    {
        DontDestroyOnLoad(this);
        UnityThread.initUnityThread();
    }

    //If the User presses the Connect Button, he will get to this Method
    //It checks if the writen Username is valid
    //It also starts connecting to the Server
    public void ConnectToServer()
    {
        StartMenuUI.instanceUI.ClearAllErrors();
        if (!(StartMenuUI.instanceUI.IsUsernameVaild()))
        {
            return;
        }
        if (alreadyTriedToConnect)
        {
            ClientTCP.InitializingNetworking();
        }
        else
        {
            alreadyTriedToConnect = true;
            ClientHandleData.InitializePackets();
            ClientTCP.InitializingNetworking();
        }



    }

    // Team Red = 1
    // Team Blue = 2
    public void ChooseTeam(int team)
    {
        DataSender.SendTeamSelected(team);
    }

    //If a new Client has joined to the match, he has to spawn
    //Also the new Client gets saved in the playerList
    //TO DO: Use only one Function for spawning both Teams
    public void InstantiatePlayerRed(int packetID, string playerData)
    {
        string spawnpoint = "SpawnRed" + packetID;
        string[] splitedData = playerData.Split(':');
        if (!playerList.ContainsKey(splitedData[0]))
        {
            NewClientsJoins(splitedData[0]);
        }
        Clients newPlayer = playerList[splitedData[0]];
        newPlayer.gameObject = Instantiate(playerPref, GameObject.Find(spawnpoint).transform.position, GameObject.Find(spawnpoint).transform.rotation);
        newPlayer.gameObject.name = newPlayer.username;

        //localPlayer - should i switch to this cam
        if (newPlayer.isLocalPlayer)
        {
            StartMenuUI.instanceUI.enableClient();

            Transform sphere = newPlayer.gameObject.transform.Find("Graphics/Sphere");
            Transform gun = newPlayer.gameObject.transform.Find("UI/ClientCamera/RocketLauncher/Gun");

            sphere.gameObject.layer = 11;
            gun.gameObject.layer = 13;
        }
    }

    public void InstantiatePlayerBlue(int packetID, string playerData)
    {
        string spawnpoint = "SpawnBlue" + packetID;
        string[] splitedData = playerData.Split(':');
        if (!playerList.ContainsKey(splitedData[0]))
        {
            NewClientsJoins(splitedData[0]);
        }
        Clients newPlayer = playerList[splitedData[0]];
        newPlayer.gameObject = Instantiate(playerPref, GameObject.Find(spawnpoint).transform.position, GameObject.Find(spawnpoint).transform.rotation);
        newPlayer.gameObject.name = newPlayer.username;

        //localPlayer - should i switch to this cam
        if (newPlayer.isLocalPlayer)
        {
            StartMenuUI.instanceUI.enableClient();

            Transform sphere = newPlayer.gameObject.transform.Find("Graphics/Sphere");
            Transform gun = newPlayer.gameObject.transform.Find("UI/ClientCamera/RocketLauncher/Gun");

            sphere.gameObject.layer = 11;
            gun.gameObject.layer = 13;
        }
    }


    //If a Client has disconnected, he has to be destroyed from the Match
    //The Client gets than removed from the playerList also
    public void DestroyClient(string clientMsg)
    {
        string[] splitedData = clientMsg.Split(':');
        GameObject player = playerList[splitedData[0]].gameObject;
        Destroy(player);

        playerList.Remove(splitedData[0]);
    }

    //This Function cordinates the transmitted Positon and Rotation from the Server of every Client
    public void MoveAllClient(string msg)
    {
        //usernameID + ":" + position + ":" + rotationPlayer + ":" + rotationCamera
        string[] data = msg.Split(':');

        //usernameID
        GameObject player = playerList[data[0]].gameObject;
        Camera clientCamera = player.gameObject.GetComponentInChildren<Camera>();

        //position: x | y | z
        string[] dataPosition = data[1].Split('|');

        Vector3 position = new Vector3(
            float.Parse(dataPosition[0]),
            float.Parse(dataPosition[1]),
            float.Parse(dataPosition[2]));

        player.transform.position = position;

        //rotationPlayer
        string[] dataRotationPlayer = data[2].Split('|');

        Vector3 rotationPlayer = new Vector3(
            float.Parse(dataRotationPlayer[0]),
            float.Parse(dataRotationPlayer[1]),
            float.Parse(dataRotationPlayer[2]));

        player.transform.eulerAngles = rotationPlayer;

        //rotationCamera
        string[] dataRotationCamera = data[3].Split('|');

        Vector3 rotationCamera = new Vector3(
            float.Parse(dataRotationCamera[0]),
            float.Parse(dataRotationCamera[1]),
            float.Parse(dataRotationCamera[2]));

        clientCamera.transform.eulerAngles = rotationCamera;

    }

    public void NewClientsJoins(string username)
    {
        Clients setUsernameOfNewClient = new Clients();
        setUsernameOfNewClient.username = username;
        playerList.Add(username, setUsernameOfNewClient);
    }

    //
    public void DisconnectFromServerString()
    {
        string disconnectLocalPlayer = playerList.FirstOrDefault(x => x.Value.isLocalPlayer == true).Key;
        string serverMessage = disconnectLocalPlayer + ":" + "has disconnected from the Server.";
        DataSender.SendDisconnetOfLocalClient(serverMessage);
    }

    public void GetRocketInfos(string serverMessage)
    {
        string[] data = serverMessage.Split(':');

        string[] dataPositionRocket = data[1].Split('|');

        //positionRocket
        Vector3 positionRocket = new Vector3(
            float.Parse(dataPositionRocket[0]),
            float.Parse(dataPositionRocket[1]),
            float.Parse(dataPositionRocket[2]));

        //rotationRocket
        string[] dataRotationRocket = data[2].Split('|');

        Vector3 rotationRocket = new Vector3(
            float.Parse(dataRotationRocket[0]),
            float.Parse(dataRotationRocket[1]),
            float.Parse(dataRotationRocket[2]));

        RocketLauncher_Weapon.instanceRocket.SpawnOtherPlayerRocket(positionRocket, rotationRocket);
    }

    public void localPlayerDied(string deathCause)
    {
        string deadLocalPlayer = playerList.FirstOrDefault(x => x.Value.isLocalPlayer == true).Key;
        string serverMessage = deadLocalPlayer + ":" + "has been killed through" + ":" + deathCause;
        DataSender.CLocalPlayerDied(serverMessage);
    }

    public void OtherPlayerDamagedHasToBeSend(float damageAmount, string username)
    {
        string deadLocalPlayer = playerList.FirstOrDefault(x => x.Value.isLocalPlayer == true).Key;
        string serverMessage = username + ":" + "has taken" + ":" + damageAmount;
        DataSender.COtherPlayerHasTakenDamage(serverMessage);
    }

    public void GivePlayerDamage(string damageInfo)
    {
        string[] data = damageInfo.Split(':');
        Debug.Log(damageInfo);
        PlayerHealth.instanceHealth.TakeDamage(float.Parse(data[2]), data[0]);
    }

}
 