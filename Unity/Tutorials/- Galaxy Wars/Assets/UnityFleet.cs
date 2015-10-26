using UnityEngine;
using System.Collections;

public class UnityFleet : MonoBehaviour {


  bool destroyed = false;
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

  public Vector3 Position {
    set { transform.position = value; }
    get { return this.transform.position; } }


  public static UnityFleet Instantiate(Vector3 p) {
    var object_fleet = GameObject.Instantiate(Resources.Load("Fleet"), p, Quaternion.identity) as GameObject;
    return object_fleet.GetComponent<UnityFleet>();

  }
}
                                                                                                                                                                