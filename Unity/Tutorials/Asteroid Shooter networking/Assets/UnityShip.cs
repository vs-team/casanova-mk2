using UnityEngine;
using System.Collections.Generic;
using Lidgren.Network;

public class UnityShip : MonoBehaviour
{

  public class ShipNetworkBuffer
  {
    public bool IsLocal { get; set; }
    public static Dictionary<int, UnityShip> NetworkContext { get; } = new Dictionary<int, UnityShip>();
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
    public bool HasPositionValue { get; set; } = false;
  }

  public ShipNetworkBuffer Buffer { get; private set; }

  void Start()
  {
    Buffer = new ShipNetworkBuffer();
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
              