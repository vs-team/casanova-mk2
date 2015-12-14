using UnityEngine;
using System.Collections.Generic;
using Lidgren.Network;

public class UnityShip : MonoBehaviour
{

  public class ShipNetworkBuffer
  {
    public NetClient client;
    public UnityNetworkManager manager;
    public bool IsLocal { get; set; }
    public static Dictionary<int, UnityShip> NetworkContext;
    public bool HasPositionValue { get; set; }

    public ShipNetworkBuffer(bool local)
    {
      IsLocal = local;
      HasPositionValue = false;
      NetworkContext = new Dictionary<int, UnityShip>();
      client = GameObject.Find("NetworkManager").GetComponent<UnityNetworkManager>().Client;
      Debug.Log(client);
      manager = GameObject.Find("NetworkManager").GetComponent<UnityNetworkManager>();
    }

    public void networkUpdate(UnityShip s)
    {
      UnityNetworkManager.Send<int>(1, client, UnityNetworkManager.BuildMessage);
      UnityNetworkManager.Send<Vector3>(s.Position, client, UnityNetworkManager.BuildMessage);
    }


    public static void addOrUpdate(UnityShip s)
    {
      if(NetworkContext.ContainsKey(s.GetHashCode()))
      {
        NetworkContext[s.GetHashCode()] = s;
      }
      else
      {
        NetworkContext.Add(s.GetHashCode(), s);
      }
    }

    public static void testAdd(UnityShip s, int i)
    {
      NetworkContext.Add(i, s);
    }
    


    public Vector3 Position
    {
      get
      {
        HasPositionValue = false;
        return Position;
      }
      set
      {
        HasPositionValue = true;
        Position = value;
      }
    }
    
  }

  public ShipNetworkBuffer Buffer { get; private set; }

  void Start()
  {
    Buffer = new ShipNetworkBuffer(true   );
  }

  public UnityNetworkManager Manager
  {
    get { return Buffer.manager; }
  }

  void Update()
  {
    UnityShip.ShipNetworkBuffer.addOrUpdate(this);
    //Buffer.networkUpdate(this);
    Debug.Log("sent message.");
  }

  bool hit;
  public static UnityShip Instantiate(Vector3 pos)
  {
    GameObject ship = GameObject.Instantiate(Resources.Load("Ship"), pos, Quaternion.identity) as GameObject;
    ship.GetComponent<UnityShip>().hit = false;
    UnityShip s = ship.GetComponent<UnityShip>();
    return s;
  }

  public Vector3 Position
  {
    get { return this.gameObject.transform.position; }
    set { this.gameObject.transform.position = value; }
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == "Asteroid")
    {
      hit = true;
    }
  }

  public bool Hit
  {
    get { return hit; }
    set { hit = value; }
  }

}
                                                                 