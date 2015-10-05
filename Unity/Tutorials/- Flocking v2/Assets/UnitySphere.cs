using UnityEngine;
using System.Collections;

public class UnitySphere : MonoBehaviour
{

  public Quaternion Rotation
  {
    get { return gameObject.transform.rotation; }
    set { gameObject.transform.rotation = value; }
  }

  public Vector3 Position
  {
    get { return gameObject.transform.position; }
    set { gameObject.transform.position = value; }
  }

  public static UnitySphere Instantiate(Vector3 location)
  {
    var b = GameObject.Instantiate(Resources.Load("Sphere"), location, Quaternion.identity) as GameObject;
    return b.GetComponent<UnitySphere>();
  }

}
                                                                                                                                                                           