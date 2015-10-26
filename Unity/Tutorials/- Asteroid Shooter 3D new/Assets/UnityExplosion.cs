using UnityEngine;
using System.Collections;

public class UnityExplosion : MonoBehaviour
{
  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }


  // Use this for initialization
  void Start()
  {
  }

  // Update is called once per frame
  void Update()
  {

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

  public static UnityExplosion Instantiate(Vector3 position)
  {
    var p = GameObject.Instantiate(Resources.Load("Explosion"), position, Quaternion.identity) as GameObject;

    return p.GetComponent<UnityExplosion>();
  }

}
                           