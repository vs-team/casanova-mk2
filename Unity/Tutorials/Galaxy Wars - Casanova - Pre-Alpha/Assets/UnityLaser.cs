using UnityEngine;
using System.Collections;

public class UnityLaser : MonoBehaviour {

	// Use this for initialization
  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  public static UnityLaser Instantiate(Vector3 startPos, Vector3 target)
  {
    var laser = GameObject.Instantiate(Resources.Load("Prefabs/Done_Bolt"), startPos, Quaternion.LookRotation(startPos - target)) as GameObject;
                

    var unity_laser = laser.GetComponent<UnityLaser>();
    //unity_laser.Rotation = target;

    return unity_laser;
  }
  public Vector3 Rotation
  {
    set { this.transform.rotation = Quaternion.LookRotation(value); }
  }

  public bool IsCollided
  {
    set
    {
      Instantiate(Resources.Load("Prefabs/ExplosionLaser"), this.transform.position, Quaternion.identity);
      this.Destroyed = true;
    }
  }

  private bool destroyed;
  public bool Destroyed
  {
    get { return destroyed; }
    set
    {
      destroyed = value;
      if (destroyed)
        GameObject.Destroy(gameObject);
    }
  }

}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      