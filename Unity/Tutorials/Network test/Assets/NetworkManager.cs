using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NetworkManager : MonoBehaviour 
{
  private const string typeName = "CasanovaNetworkingTest";
  private const string gameName = "CasanovaRoom";
  private GameObject playerObject;
  private HostData[] hostList;

  private void RefreshHostList()
  {
    Debug.Log("Refreshing host list for " + typeName);
    MasterServer.RequestHostList(typeName);
  }

  void OnMasterServerEvent(MasterServerEvent msEvent)
  {
    if (msEvent == MasterServerEvent.HostListReceived)
    {
      Debug.Log("Host list received");
      hostList = MasterServer.PollHostList();
      Debug.Log(hostList.Length);
    }
  }

	private void StartServer()
  {
    Debug.Log("Starting server...");
    //MasterServer.ipAddress = "localhost";
    Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
    MasterServer.RegisterHost(typeName, gameName);
    Network.Instantiate(Resources.Load("Casanova game logic"), Vector3.zero, Quaternion.identity, 0);

  }

  private void JoinServer(HostData hostData)
  {
    Debug.Log("Connecting to server...");
    Network.Connect(hostData);

  }

  void OnConnectedToServer()
  {
    Debug.Log("Server joined!");
    //SpawnPlayer();
    //Network.Instantiate(Resources.Load("Casanova game logic"), Vector3.zero, Quaternion.identity, 0);

  }

  void OnServerInitialized()
  {
    Debug.Log("Server Initialized!");
    SpawnPlayer();
  }

  private void SpawnPlayer()
  {
    //Network.Instantiate(Resources.Load("Casanova game logic"), Vector3.zero, Quaternion.identity, 0);
    //playerObject = Resources.Load("Cube") as GameObject;
    //Network.Instantiate(Resources.Load("Cube"), new Vector3(77.5f, 1.0f, 84.6f), Quaternion.identity, 0);
  }

  void OnGUI()
  {
    if (!Network.isClient && !Network.isServer)
    {
      if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
        StartServer();

      if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
      {
        RefreshHostList();
      }

      if (hostList != null && hostList.Length > 0)
      {
        for (int i = 0; i < hostList.Length; i++)
        {
          if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
            JoinServer(hostList[i]);
        }
      }
    }
  }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 