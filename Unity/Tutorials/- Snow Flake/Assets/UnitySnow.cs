using UnityEngine;
using System.Collections;

public class UnitySnow : MonoBehaviour {

  public Vector3 Position
  {
    get { return this.transform.position; }
    set { this.transform.position = value; }
  }

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

  public static UnitySnow Instantiate(Vector3 position)
  {
    var snow = GameObject.Instantiate(Resources.Load("SnowFlake"), position, Quaternion.LookRotation(UnityEngine.Random.insideUnitSphere)) as GameObject;
    return snow.GetComponent<UnitySnow>();
  }
}
                                                                                              