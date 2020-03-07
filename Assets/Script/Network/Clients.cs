using UnityEngine;

namespace Assets.Script.Network
{
    public class Clients
    {
        public int connectionID;
        public int ipAddress;
        public string username;
        public bool OnServerAlready = false;
        public int packetID;
        public GameObject gameObject;
        public bool isLocalPlayer = false;
    }
}
