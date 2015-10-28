using UnityEngine;
using System.Collections;
using Lidgren.Network;
using System.Net;

public class UnityGameClient : MonoBehaviour {

    private NetClient netClient;

    public static UnityGameClient Find()
    {
        GameObject gameClient = GameObject.Find("GameClient");
        var gameClientScript = gameClient.GetComponent<UnityGameClient>();
        NetPeerConfiguration config = new NetPeerConfiguration("SpaceShooter");
        gameClientScript.netClient = new NetClient(config);
        gameClientScript.netClient.Start();
        return gameClientScript;
    }

    public bool Connect
    {
        get
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(GameSettings.IPAddress.ToString()), GameSettings.PortNumber);
            netClient.Connect(endPoint);
            if (netClient.GetConnection(endPoint) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             