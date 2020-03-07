using Assets.Script;
using UnityEngine;

/*
 * This class instantiates a rocket from the RocketLauncher
 */

public class RocketLauncher_Weapon : MonoBehaviour
{
    public GameObject Rocket;
    public GameObject RocketLauncher;
    public GameObject spawnPointForRocket;
    public GameObject Player;
    string sendPositionRocket = "";
    string sendRotationRocket = "";
    string serverMessage = "";
    GameObject newRocket;
    public Camera Clientcamera;

    public static RocketLauncher_Weapon instanceRocket;

    void Awake()
    {
        instanceRocket = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Clientcamera.enabled == true)
        {
            newRocket = Instantiate(Rocket, spawnPointForRocket.transform.position, RocketLauncher.transform.rotation);
            newRocket.transform.SetParent(Player.transform);

            sendPositionRocket = newRocket.transform.position.x + "|" + newRocket.transform.position.y + "|" + newRocket.transform.position.z;
            sendRotationRocket = newRocket.transform.eulerAngles.x + "|" + newRocket.transform.eulerAngles.y + "|" + newRocket.transform.eulerAngles.z;
            serverMessage = Player.name + ":" + sendPositionRocket + ":" + sendRotationRocket;
            DataSender.CRocketHasBeenSpawned(serverMessage);
        }
    }

    public void SpawnOtherPlayerRocket(Vector3 positon, Vector3 rotation)
    {
        newRocket = Instantiate(Rocket, positon, Quaternion.Euler(rotation));
    }
}
