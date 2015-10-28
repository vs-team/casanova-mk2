using UnityEngine;
using System.Collections;

public class UnityShipExplosion : MonoBehaviour {

  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  public bool destroyed = false;
  public bool Destroyed
  {
    get
    {
      return destroyed;
    }
    set
    {
      if (!ps.IsAlive())
      {
        Destroy(gameObject);
        destroyed = true;
      }
      else destroyed = false;
    }
  }

  ParticleSystem ps; // <----
  public ParticleSystem Ps
  {
    get
    {
      ps = gameObject.GetComponent<ParticleSystem>();
      return ps;
    }
  }

  public static UnityShipExplosion Instantiate(Vector3 position)
  {
    var p = GameObject.Instantiate(Resources.Load("ShipExplosion"), position, Quaternion.identity) as GameObject;

    return p.GetComponent<UnityShipExplosion>();
  }

}
                                                                                                                                                                                                                                            