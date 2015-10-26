using UnityEngine;
using System.Collections;

public class UnityAsteroid : MonoBehaviour {

  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  bool destroyed = false;
  public bool Destroyed
  {
    get { return destroyed; }
    set
    {
      destroyed = value;
      if (destroyed)
      {
        GameObject.Destroy(gameObject);
      }
    }
  }

  public Quaternion Rotation
  {
    get { return this.transform.rotation; }
    set { this.transform.rotation = value; }
  }

  public static UnityAsteroid Instantiate(Vector3 position)
  {
    var s = GameObject.Instantiate(Resources.Load("Asteroid"), position, Quaternion.identity) as GameObject;
    return s.GetComponent<UnityAsteroid>();
  }
}
                                                                                                                                                                                                                                                                                                                                                                               