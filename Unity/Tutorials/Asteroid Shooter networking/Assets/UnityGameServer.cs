using UnityEngine;
using System.Collections;
using Lidgren.Network;
using System.Net;

public class UnityGameServer : MonoBehaviour {

    private NetServer netServer;

    public static UnityGameServer Find()
    {
        GameObject netServer = GameObject.Find("GameServer");
        UnityGameServer netServerScript = netServer.GetComponent<UnityGameServer>();
        NetPeerConfiguration config = new NetPeerConfiguration("SpaceShooter");
        config.Port = GameSettings.PortNumber;
        netServerScript.netServer = new NetServer(config);
        return netServerScript;
    }

    public bool Host
    {
        get
        {
            netServer.Start();
            return true;
        }
    }

    public int Connections
    {
        get { return netServer.ConnectionsCount; }
    }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           